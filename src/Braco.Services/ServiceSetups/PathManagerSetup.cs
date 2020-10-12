using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Services
{
	/// <summary>
	/// Used for setting up <see cref="PathManager"/> as <see cref="IPathManager"/>.
	/// </summary>
	public class PathManagerSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = nameof(PathManager);

		/// <summary>
		/// Key for the current culture configuration value inside current section.
		/// </summary>
		public const string AppNameKey = "AppName";

		/// <inheritdoc/>
		public string ConfigurationSection { get; } = SectionName;

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<IPathManager>(new PathManager(section[AppNameKey]));
		}
	}
}
