using Braco.Services;
using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for initializing the project's <see cref="IResourceManager"/>
	/// with all of the <see cref="ResourceGetter"/>s that have been defined
	/// and added to the service collection.
	/// </summary>
	public class ResourceManagerInitializer : IInitializeAfterBuildingServices
	{
		private readonly IResourceManager _resourceManager;

		/// <summary>
		/// Not used.
		/// </summary>
		public string ConfigurationSection { get; }

		/// <summary>
		/// Creates an instance of the initializer with project's resource manager.
		/// </summary>
		/// <param name="resourceManager">Resource manager used by the project.</param>
		public ResourceManagerInitializer(IResourceManager resourceManager)
		{
			_resourceManager = resourceManager;
		}

		/// <inheritdoc/>
		public void Initialize(IServiceProvider provider, IConfiguration configuration, IConfigurationSection section)
		{
			ReflectionUtilities
				.FindAssignableTypes(typeof(ResourceGetter))
				.Where(getterType => !getterType.IsAbstract)
				.Select(getterType => provider.GetService(getterType))
				.Where(getter => getter != null)
				.ForEach(getter => _resourceManager.Set(getter.GetType(), getter));
		}
	}
}
