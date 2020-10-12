using Braco.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for setting up <see cref="ImageGetter"/>.
	/// </summary>
	public class ImageGetterSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = nameof(ImageGetter);

		/// <summary>
		/// Key for the location configuration value inside current section.
		/// </summary>
		public const string LocationKey = "Location";

		/// <inheritdoc/>
		public string ConfigurationSection { get; } = SectionName;

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			var location = section[LocationKey] ?? ImageGetter.DefaultLocation;

			var getter = new ImageGetter(location);

			services.AddSingleton(getter);
		}
	}
}

