using Braco.Utilities;
using Braco.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Braco.Services
{
	/// <summary>
	/// Used for setting up various services and their provider.
	/// </summary>
	public class Initializer
	{
		#region Constants

		/// <summary>
		/// Name of the section within configuration that will contain data
		/// required for setting up services.
		/// </summary>
		public const string InitializerSectionName = "Initializer";

		/// <summary>
		/// Read-only configuration file name.
		/// </summary>
		public const string ReadOnlyConfigName = "appsettings";
		/// <summary>
		/// Read-only configuration file extension.
		/// </summary>
		public const string ReadOnlyConfigExtension = ".json";

		/// <summary>
		/// General read-only configuration file.
		/// </summary>
		public const string ReadOnlyGeneralConfig = ReadOnlyConfigName + ReadOnlyConfigExtension;
		/// <summary>
		/// Read-only configuration file for development environment.
		/// </summary>
		public const string ReadOnlyDebuggingConfig = ReadOnlyConfigName + ".Development" + ReadOnlyConfigExtension;
		/// <summary>
		/// Read-only configuration file for production environment.
		/// </summary>
		public const string ReadOnlyNonDebuggingConfig = ReadOnlyConfigName + ".Production" + ReadOnlyConfigExtension;

		#endregion

		private readonly IList<Type> _postServiceBuildInitializerTypes = new List<Type>();

		/// <summary>
		/// Collection of implementations of service setuping members.
		/// </summary>
		protected readonly IList<ISetupService> _serviceSetups = new List<ISetupService>();

		/// <summary>
		/// Collection of implementations of initializers triggered after the service
		/// provider has been built.
		/// </summary>
		protected readonly IList<IInitializeAfterBuildingServices> _postServiceBuildInitializers = new List<IInitializeAfterBuildingServices>();

		/// <summary>
		/// Service collection used for setting up the provider.
		/// </summary>
		public IServiceCollection Services { get; set; }

		/// <summary>
		/// Read-only configuration used in the project.
		/// </summary>
		public IConfiguration Configuration { get; set; }

		/// <summary>
		/// Creates an instance of the initializer.
		/// </summary>
		/// <param name="addEverything">Specifies if <see cref="AddEverything"/>
		/// should be called instantly.</param>
		public Initializer(bool addEverything = true)
		{
			if (addEverything)
			{
				AddEverything();
			}
		}

		/// <summary>
		/// Used for setting up the services and <see cref="DI.Provider"/>.
		/// </summary>
		/// <param name="configure">Optional method for providing additional
		/// setup for the <see cref="ConfigurationBuilder"/>.</param>
		public void Run(Action<ConfigurationBuilder> configure = null)
		{
			// Make sure we have a configuration
			Configuration ??= LoadConfiguration(configure) ?? throw new Exception($"Configuration is required. Either let the default configuration be setup, assign {nameof(Configuration)} property a value or return valid one in the {nameof(LoadConfiguration)} method.");

			// Make sure we have a service collection
			Services ??= new ServiceCollection();

			// Make sure IConfiguration is part of the service collection
			Services.AddSingleton(Configuration);

			// Get the Initializer section
			var baseSection = Configuration.GetSection(InitializerSectionName);

			// Get the section for service setups
			var serviceSetupsSection = baseSection.GetSection(ISetupService.ServicesSetupSection);

			// Setup services that have been specified
			foreach (var serviceSetup in _serviceSetups)
			{
				serviceSetup.Setup(Services, Configuration, serviceSetupsSection.GetSection(serviceSetup.ConfigurationSection));
			}

			// Setup the provider
			DI.Provider ??= Services.BuildServiceProvider();

			// Go through initializer types...
			foreach (var postServiceBuildInitializerType in _postServiceBuildInitializerTypes)
			{
				// Create an instance and support constructor injection
				var instance = (IInitializeAfterBuildingServices)ActivatorUtilities.CreateInstance(DI.Provider, postServiceBuildInitializerType);

				// Add the initializer to the collection
				_postServiceBuildInitializers.Add(instance);
			}

			// Clear the given initializer types after they've been instantiated
			_postServiceBuildInitializerTypes.Clear();

			// Get the section for initializers
			var initializersSection = baseSection.GetSection(IInitializeAfterBuildingServices.InitializersSection);

			// Initialize other services after the provider has been setup
			foreach (var initializer in _postServiceBuildInitializers)
			{
				initializer.Initialize(DI.Provider, Configuration, initializersSection.GetSection(initializer.ConfigurationSection));
			}
		}

		/// <summary>
		/// Helper for adding multiple setups at once.
		/// </summary>
		/// <param name="serviceSetups">Setups to add to <see cref="_serviceSetups"/>.</param>
		public void AddServiceSetups(params ISetupService[] serviceSetups)
		{
			foreach (var serviceSetup in serviceSetups)
			{
				if (serviceSetup == null) continue;

				_serviceSetups.Add(serviceSetup);
			}
		}

		/// <summary>
		/// Helper for adding multiple setups at once by type.
		/// </summary>
		/// <param name="serviceSetupTypes">Service setup types to instantiate and add to <see cref="_serviceSetups"/>.</param>
		public void AddServiceSetups(params Type[] serviceSetupTypes)
		{
			foreach (var serviceSetupType in serviceSetupTypes)
			{
				// Make sure we only deal with non abstract types
				if (serviceSetupType?.IsAbstract != false) continue;

				// Create an instance and add it
				_serviceSetups.Add((ISetupService)Activator.CreateInstance(serviceSetupType));
			}
		}

		/// <summary>
		/// Adds all <see cref="ISetupService"/>s from current domain and creates
		/// an instance of each one by using the parameterless constructor.
		/// </summary>
		public void AddAllServiceSetups()
			=> AddServiceSetups
			(
				ReflectionUtilities
					.FindAssignableTypes(typeof(ISetupService))
					.ToArray()
			);

		/// <summary>
		/// Adds all <see cref="ISetupService"/>s from given assemblies and creates
		/// an instance of each one by using the parameterless constructor.
		/// </summary>
		/// <param name="assemblies">Assemblies from which to load all <see cref="ISetupService"/>s.</param>
		public void AddAllServiceSetups(params Assembly[] assemblies)
			=> AddServiceSetups
			(
				ReflectionUtilities
					.FindAssignableTypes(typeof(ISetupService), assemblies)
					.ToArray()
			);

		/// <summary>
		/// Helper for adding multiple initializers at once.
		/// </summary>
		/// <param name="postServiceBuildInitializers">Initializers to add to <see cref="_postServiceBuildInitializers"/>.</param>
		public void AddPostServiceBuildInitializers(params IInitializeAfterBuildingServices[] postServiceBuildInitializers)
		{
			foreach (var postServiceBuildInitializer in postServiceBuildInitializers)
			{
				if (postServiceBuildInitializer == null) continue;

				_postServiceBuildInitializers.Add(postServiceBuildInitializer);
			}
		}

		/// <summary>
		/// Helper for adding multiple initializers at once by type.
		/// </summary>
		/// <param name="postServiceBuildInitializerTypes">Initializer types to instantiate and add to <see cref="_postServiceBuildInitializers"/>.</param>
		public void AddPostServiceBuildInitializers(params Type[] postServiceBuildInitializerTypes)
		{
			foreach (var postServiceBuildInitializerType in postServiceBuildInitializerTypes)
			{
				// Make sure we only deal with non abstract types
				if (postServiceBuildInitializerType?.IsAbstract != false) continue;

				// Add the type to initializer types collection
				_postServiceBuildInitializerTypes.Add(postServiceBuildInitializerType);
			}
		}

		/// <summary>
		/// Adds all <see cref="IInitializeAfterBuildingServices"/> from current domain and creates
		/// an instance of each one. Service injection into constructor is supported.
		/// </summary>
		public void AddAllPostServiceBuildInitializers()
			=> AddPostServiceBuildInitializers
			(
				ReflectionUtilities
					.FindAssignableTypes(typeof(IInitializeAfterBuildingServices))
					.ToArray()
			);

		/// <summary>
		/// Adds all <see cref="IInitializeAfterBuildingServices"/> from given assemblies and creates
		/// an instance of each one. Service injection into constructor is supported.
		/// </summary>
		/// <param name="assemblies">Assemblies from which to load all <see cref="IInitializeAfterBuildingServices"/>.</param>
		public void AddAllPostServiceBuildInitializers(params Assembly[] assemblies)
			=> AddPostServiceBuildInitializers
			(
				ReflectionUtilities
					.FindAssignableTypes(typeof(IInitializeAfterBuildingServices), assemblies)
					.ToArray()
			);

		/// <summary>
		/// Helper that calls <see cref="AddAllServiceSetups"/>
		/// and <see cref="AddAllPostServiceBuildInitializers"/>.
		/// </summary>
		public void AddEverything()
		{
			AddAllServiceSetups();
			AddAllPostServiceBuildInitializers();
		}

		/// <summary>
		/// Removes the service setups of given types.
		/// </summary>
		/// <param name="serviceSetupsToRemove">Types of service setups to remove.</param>
		/// <returns>How many of them were removed.</returns>
		public int RemoveServiceSetups(params Type[] serviceSetupsToRemove)
		{
			var removed = 0;

			for (int i = _serviceSetups.Count - 1; i >= 0; i--)
			{
				var current = _serviceSetups[i];

				if (current.GetType().In(serviceSetupsToRemove))
				{
					_serviceSetups.RemoveAt(i);
					removed++;
				}
			}

			return removed;
		}

		/// <summary>
		/// Removes the initializers of given types.
		/// </summary>
		/// <param name="initializersToRemove">Types of initializers to remove.</param>
		/// <returns>How many of them were removed.</returns>
		public int RemovePostServiceBuildInitializers(params Type[] initializersToRemove)
		{
			var removed = 0;

			for (int i = _postServiceBuildInitializers.Count - 1; i >= 0; i--)
			{
				var current = _postServiceBuildInitializers[i];

				if (current.GetType().In(initializersToRemove))
				{
					_postServiceBuildInitializers.RemoveAt(i);
					removed++;
				}
			}

			return removed;
		}

		/// <summary>
		/// Used for loading the read-only configuration.
		/// </summary>
		/// <param name="configure">Optional method for providing additional
		/// setup for the <see cref="ConfigurationBuilder"/>.</param>
		/// <returns>Configuration instance.</returns>
		protected virtual IConfiguration LoadConfiguration(Action<ConfigurationBuilder> configure = null)
		{
			var configBuilder = new ConfigurationBuilder();

			configBuilder.SetBasePath(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location));

			configBuilder.AddJsonFile(ReadOnlyGeneralConfig, optional: true, reloadOnChange: true);

			var otherConfig = Debugger.IsAttached ? ReadOnlyDebuggingConfig : ReadOnlyNonDebuggingConfig;

			configBuilder.AddJsonFile(otherConfig, optional: true, reloadOnChange: true);

			configure?.Invoke(configBuilder);

			return configBuilder.Build();
		}

		/// <summary>
		/// Used for running the specific initializer implementation.
		/// </summary>
		/// <typeparam name="TInitializer">Type of initializer to use.</typeparam>
		/// <param name="configure">Optional method for providing additional
		/// setup for the <see cref="ConfigurationBuilder"/>.</param>
		public static void RunSpecificInitializer<TInitializer>(Action<ConfigurationBuilder> configure = null)
			where TInitializer : Initializer, new()
		{
			var initializer = new TInitializer();

			initializer.Run(configure);
		}

		/// <summary>
		/// Used for running the <see cref="Initializer"/> with default setup.
		/// </summary>
		/// <param name="configure">Optional method for providing additional
		/// setup for the <see cref="ConfigurationBuilder"/>.</param>
		public static void RunDefaultInitializer(Action<ConfigurationBuilder> configure = null)
		{
			var initializer = new Initializer();

			initializer.Run(configure);
		}
	}
}
