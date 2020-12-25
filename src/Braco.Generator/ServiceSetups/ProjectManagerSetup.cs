using Braco.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Generator
{
	public class ProjectManagerSetup : ISetupService
	{
		public string ConfigurationSection { get; }

		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<ProjectManager>();
		}
	}
}
