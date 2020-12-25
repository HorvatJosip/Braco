using Braco.Services;
using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for setting up localized collections (types that inherit <see cref="IHaveLocalizedCollection"/>) to
	/// initialize and to update whenever the language changes (<see cref="ILocalizer.LanguageChanged"/>).
	/// </summary>
	public class LocalizedCollectionsSetup : ISetupService
	{
		/// <summary>
		/// Not used.
		/// </summary>
		public string ConfigurationSection { get; }

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			var types = ReflectionUtilities
				.FindAssignableTypes(typeof(IHaveLocalizedCollection))
				.Where(type => !type.IsAbstract);

			types.ForEach(type => services.AddSingleton(type, provider =>
			{
				var instance = (IHaveLocalizedCollection)ActivatorUtilities.CreateInstance(provider, type);

				var localizer = provider.GetService<ILocalizer>();

				instance.Fill(localizer);

				localizer.LanguageChanged += (_, __) => instance.Fill(localizer);

				return instance;
			}));
		}
	}
}
