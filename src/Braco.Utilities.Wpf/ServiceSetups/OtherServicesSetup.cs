using Braco.Services;
using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Sets up the service implementations for the specific abstractions:
	/// <para><see cref="ChooserDialogsService"/> for <see cref="IChooserDialogsService"/>.</para>
	/// <para><see cref="WpfMethodService"/> for <see cref="IMethodService"/>.</para>
	/// <para><see cref="WindowService"/> for <see cref="IWindowService"/>.</para>
	/// <para>Note: adding to the service collection for the same abstraction twice will make
	/// the second call for adding the service override the first one.</para>
	/// </summary>
	public class OtherServicesSetup : ISetupService
	{
		/// <summary>
		/// Not used.
		/// </summary>
		public string ConfigurationSection { get; }

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<IChooserDialogsService, ChooserDialogsService>();
			services.AddSingleton<IMethodService, WpfMethodService>();
			services.AddSingleton<IWindowService, WindowService>();
		}
	}
}
