using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Services
{
	/// <summary>
	/// Defines that the inheriting member will setup one or more services.
	/// </summary>
	public interface ISetupService : IHaveConfigurationSection
	{
		/// <summary>
		/// Name of the section where configuration for <see cref="ISetupService"/>s
		/// should be placed.
		/// </summary>
		public const string ServicesSetupSection = "Services";

		/// <summary>
		/// Used for setting up the desired service(s).
		/// </summary>
		/// <param name="services">Collection of services to add the service(s) to.</param>
		/// <param name="configuration">Read-only configuration.</param>
		/// <param name="section">Section in the configuration dedicated to this <see cref="ISetupService"/>.</param>
		void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section);
	}
}
