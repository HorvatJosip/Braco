using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Braco.Services
{
	/// <summary>
	/// Used for setting up <see cref="FileConfigurationService"/> as <see cref="IConfigurationService"/>.
	/// </summary>
	public class FileConfigurationServiceSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = "ConfigurationService";

		/// <summary>
		/// Key for the file path configuration value inside current section.
		/// </summary>
		public const string FilePathKey = "FilePath";
		/// <summary>
		/// Key for the initial configuration section nested inside current section.
		/// </summary>
		public const string InitialConfigurationSectionName = "InitialConfiguration";

		/// <inheritdoc/>
		public string ConfigurationSection { get; } = SectionName;

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<IConfigurationService>(provider =>
			{
				string filePath = null;
				IDictionary<string, object> initialConfiguration = null;

				filePath = section[FilePathKey];

				var configuration = section.GetSection(InitialConfigurationSectionName);

				if (configuration.Exists())
				{
					initialConfiguration = configuration.GetChildren().ToDictionary(x => x.Key, x => (object)x.Value);
				}

				return new FileConfigurationService(filePath, initialConfiguration, provider.GetService<ISecurityService>());
			});
		}
	}
}
