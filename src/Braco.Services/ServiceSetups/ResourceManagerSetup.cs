using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Braco.Services
{
	/// <summary>
	/// Used for setting up <see cref="ResourceManager"/> as <see cref="IResourceManager"/>.
	/// </summary>
	public class ResourceManagerSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = nameof(ResourceManager);

		/// <summary>
		/// Key for the location configuration value inside current section.
		/// </summary>
		public const string LocationKey = "Location";
		/// <summary>
		/// Key for the location separator configuration value inside current section.
		/// </summary>
		public const string LocationSeparatorKey = "LocationSeparator";
		/// <summary>
		/// Key for the flag configuration value inside current section that indicates
		/// if all of the <see cref="ResourceGetter"/>s should be added from current app domain.
		/// </summary>
		public const string AddFromAppDomainKey = "AddFromAppDomain";

		/// <inheritdoc/>
		public string ConfigurationSection { get; } = SectionName;

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			var resourceManager = new ResourceManager
			(
				location: section[LocationKey] ?? ResourceManager.DefaultLocation,
				locationSeparator: section[LocationSeparatorKey] ?? ResourceManager.DefaultLocationSeparator
			);

			if (bool.TryParse(section[AddFromAppDomainKey], out var add) && add)
			{
				resourceManager.AddFromAppDomain(AppDomain.CurrentDomain);
			}

			services.AddSingleton<IResourceManager>(resourceManager);
		}
	}
}
