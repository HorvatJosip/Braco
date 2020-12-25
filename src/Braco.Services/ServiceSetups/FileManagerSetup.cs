using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Services
{
	/// <summary>
	/// Used for setting up <see cref="FileManager"/> as <see cref="IFileManager"/>.
	/// </summary>
	public class FileManagerSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = nameof(FileManager);

		/// <summary>
		/// Key for the current culture configuration value inside current section.
		/// </summary>
		public const string AppNameKey = "AppName";

		/// <inheritdoc/>
		public string ConfigurationSection { get; } = SectionName;

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<IFileManager>(new FileManager(section[AppNameKey]));
		}
	}
}
