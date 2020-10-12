using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Braco.Services
{
	/// <summary>
	/// Used for setting up <see cref="JsonLocalizer"/> as <see cref="ILocalizer"/>.
	/// </summary>
	public class JsonLocalizerSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = "Localizer";

		/// <summary>
		/// Key for the current culture configuration value inside current section.
		/// </summary>
		public const string CurrentCultureKey = "CurrentCulture";
		/// <summary>
		/// Name of the section for the culture collection inside current section.
		/// This should be an array of strings.
		/// </summary>
		public const string CulturesSectionName = "Cultures";

		/// <inheritdoc/>
		public string ConfigurationSection { get; } = SectionName;

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<ILocalizer>(provider =>
			{
				var currentCulture = section[CurrentCultureKey];
				var cultures = section.GetSection(CulturesSectionName).Get<string[]>();

				// Get the current culture's name
				currentCulture ??= CultureInfo.CurrentCulture.Name;

				// If it isn't specified in the available cultures...
				if (currentCulture.In(cultures) == false)
					// Just use the first one
					currentCulture = cultures[0];

				return new JsonLocalizer(provider.GetService<IPathManager>(), currentCulture, cultures);
			});
		}
	}
}
