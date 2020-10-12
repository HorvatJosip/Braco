using Braco.Services;
using Braco.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Adds <see cref="ContentViewModel"/>s, <see cref="PageViewModel"/>s and
	/// <see cref="WindowViewModel"/>s to the service collection.
	/// </summary>
	public class ViewModelsSetup : ISetupService
	{
		/// <summary>
		/// Not used.
		/// </summary>
		public string ConfigurationSection { get; }

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			ReflectionUtilities
				.FindAssignableTypes(typeof(ContentViewModel))
				.Where(type => !type.In(typeof(ContentViewModel), typeof(PageViewModel), typeof(WindowViewModel)))
				.ForEach(type => services.AddSingleton(type));
		}
	}
}
