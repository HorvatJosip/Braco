using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Services
{
	/// <summary>
	/// Sets up the service implementations for the specific abstractions:
	/// <para><see cref="FileAuthService"/> for <see cref="IAuthService"/>.</para>
	/// <para><see cref="SecurityService"/> for <see cref="ISecurityService"/>.</para>
	/// <para><see cref="WindowsProcessStarter"/> for <see cref="IProcessStarter"/>.</para>
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
			services.AddSingleton<IAuthService, FileAuthService>();
			services.AddSingleton<ISecurityService, SecurityService>();
			services.AddSingleton<IProcessStarter, WindowsProcessStarter>();
		}
	}
}
