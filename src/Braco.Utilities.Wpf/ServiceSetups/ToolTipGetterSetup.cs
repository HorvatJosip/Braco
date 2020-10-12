using Braco.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for setting up <see cref="ToolTipGetterSetup"/>.
	/// </summary>
	public class ToolTipGetterSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = nameof(ToolTipGetter);

		/// <summary>
		/// Key for the key map section nested inside current section.
		/// </summary>
		public const string KeyMapSectionName = "KeyMap";

		/// <inheritdoc/>
		public string ConfigurationSection { get; } = SectionName;

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			IDictionary<string, string> keyMap = null;

			var keyMapSection = section.GetSection(KeyMapSectionName);

			if (keyMapSection.Exists())
			{
				keyMap = keyMapSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);
			}

			var getter = new ToolTipGetter(keyMap);

			services.AddSingleton(getter);
		}
	}
}
