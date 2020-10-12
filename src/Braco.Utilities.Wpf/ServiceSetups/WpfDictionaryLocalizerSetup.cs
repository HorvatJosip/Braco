using Braco.Services;
using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for setting up <see cref="WpfDictionaryLocalizer"/> as <see cref="ILocalizer"/>.
	/// </summary>
	public class WpfDictionaryLocalizerSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = "Localizer";

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
			var cultures = section.GetSection(CulturesSectionName).Get<string[]>();

			var localizer = new WpfDictionaryLocalizer(cultures);

			services.AddSingleton<ILocalizer>(localizer);
		}
	}
}
