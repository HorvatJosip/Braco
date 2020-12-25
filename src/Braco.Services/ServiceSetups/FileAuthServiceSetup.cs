using Braco.Services;
using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for setting up <see cref="FileAuthService"/> as <see cref="IAuthService"/>.
	/// </summary>
	public class FileAuthServiceSetup : ISetupService
	{
		/// <summary>
		/// Not used.
		/// </summary>
		public string ConfigurationSection { get; }

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<IAuthService, FileAuthService>();
		}
	}
}
