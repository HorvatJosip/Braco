using Braco.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Braco.Services
{
    /// <summary>
    /// Collection of <see cref="ConfigurationItem"/>s.
    /// </summary>
    public class Configuration
    {
        private readonly IList<ConfigurationItem> configuration = new List<ConfigurationItem>();

		/// <summary>
		/// Gets configuration item by key.
		/// </summary>
		/// <param name="key">Key of the configuration item.</param>
		/// <returns>Configuration item, if it exists.</returns>
        public ConfigurationItem this[string key] => configuration.FirstOrDefault(c => c.Key == key);

		/// <summary>
		/// Creates a configuration with optional initial configuration.
		/// </summary>
		/// <param name="initialConfiguration">Initial configuration values.</param>
        public Configuration(IDictionary<string, object> initialConfiguration)
        {
            initialConfiguration?.ForEach(item => configuration.Add(new ConfigurationItem(item.Key, item.Value)));
        }

		/// <summary>
		/// Sets value of an item.
		/// </summary>
		/// <param name="key">Item's key.</param>
		/// <param name="value">Item's value.</param>
        public void SetItemValue(string key, object value)
        {
            var existing = this[key];

            if (existing == null)
                configuration.Add(new ConfigurationItem(key, value));
            else
                existing.Value = value;
        }

		/// <summary>
		/// Parses stringified configuration data.
		/// </summary>
		/// <param name="stringifiedData">Strigified data.</param>
        public void Parse(IEnumerable<string> stringifiedData)
            => stringifiedData?.ForEach(line =>
            {
                var item = ConfigurationItem.CreateFromLine(line);

                SetItemValue(item.Key, item.Value);
            });

		/// <summary>
		/// Stringifies the configuration.
		/// </summary>
		/// <returns>Stringified version of the configuration.</returns>
        public IEnumerable<string> Stringify() => configuration.Select(item => item.ConvertToLine());
    }
}
