using System;

namespace Braco.Services.Abstractions
{
    /// <summary>
    /// Used for working with configuration.
    /// </summary>
    public interface IConfigurationService
	{
		/// <summary>
		/// Gets a string from the configuration.
		/// </summary>
		/// <param name="key">Key used for getting a string from configuration.</param>
		/// <returns>Value from the setting at the given key or null if nothing is found.</returns>
		string this[string key] { get; }

		/// <summary>
		/// Fired when a setting has changed.
		/// </summary>
		event EventHandler<SettingChangedEventArgs> SettingChanged;

        /// <summary>
        /// Gets a value from the configuration.
        /// </summary>
        /// <typeparam name="T">Type of value to get.</typeparam>
        /// <param name="key">Key used to get the value from configuration.</param>
        /// <returns>Value from the configuration at given key converted into
		/// the type <typeparamref name="T"/>.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Sets a value in the configuration by key.
        /// </summary>
        /// <param name="key">Key used to identify the setting.</param>
        /// <param name="value">Value to store under the given key.</param>
		/// <returns>If the value was set or not.</returns>
        bool Set(string key, object value);

		/// <summary>
		/// Loads the configuration.
		/// </summary>
		/// <returns>If the configuration was loaded or not.</returns>
		bool Load();

		/// <summary>
		/// Saves the configuration.
		/// </summary>
		/// <returns>If the configuration was saved or not.</returns>
		bool Save();
	}
}