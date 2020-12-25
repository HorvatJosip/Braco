using Braco.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for setting up <see cref="WindowsManager"/> as <see cref="IWindowsManager"/>.
	/// </summary>
	public class WindowsManagerSetup : ISetupService
	{
		/// <summary>
		/// Not used.
		/// </summary>
		public string ConfigurationSection { get; }

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<IWindowsManager, WindowsManager>();
		}
	}
}
