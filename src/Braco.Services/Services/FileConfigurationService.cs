using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Braco.Services
{
	/// <summary>
	/// <see cref="IConfigurationService"/> implementation using file storage.
	/// </summary>
	public class FileConfigurationService : IConfigurationService
	{
		private const string defaultFilePath = "AppSettings.dat";

		private readonly string _filePath;
		private readonly ISecurityService _securityService;
		private readonly Configuration _configuration;

		/// <inheritdoc/>
		public event EventHandler<SettingChangedEventArgs> SettingChanged;

		/// <summary>
		/// Creates an instance of the service.
		/// </summary>
		/// <param name="filePath">Path to the file where configuration will reside.</param>
		/// <param name="initialConfiguration">Initial values for the configuration.</param>
		/// <param name="securityService">Service used for encrypting the configuration. If you don't provide it
		/// (null), no encryption will be used for the configuration.</param>
		public FileConfigurationService(string filePath, IDictionary<string, object> initialConfiguration, ISecurityService securityService)
		{
			_securityService = securityService;
			_filePath = filePath ?? defaultFilePath;
			_configuration = new Configuration(initialConfiguration);
		}

		/// <inheritdoc/>
		public string this[string key]
		{
			get
			{
				var configItem = _configuration[key];

				if (configItem != null) return configItem.StringValue();

				if (DI.ReadOnlyConfiguration != null) return DI.ReadOnlyConfiguration[key];

				return null;
			}
		}

		/// <inheritdoc/>
		public T Get<T>(string key)
		{
			var configItem = _configuration[key];

			if (configItem != null) return configItem.GetValue<T>();

			if (DI.ReadOnlyConfiguration != null)
			{
				var configValue = DI.ReadOnlyConfiguration[key];

				if (configValue != null) return configValue.Convert<T>();

				var configSection = DI.ReadOnlyConfiguration.GetSection(key);

				if (configSection != null) return configSection.Get<T>();
			}

			return default;
		}

		/// <inheritdoc/>
		public bool Set(string key, object value)
		{
			var oldValue = _configuration[key]?.Value;

			if (!Equals(oldValue, value))
			{
				_configuration.SetItemValue(key, value);

				SettingChanged?.Invoke(this, new SettingChangedEventArgs(key, oldValue, value));

				return true;
			}

			return false;
		}

		/// <inheritdoc/>
		public bool Load()
		{
			if (File.Exists(_filePath) == false)
				return false;

			var fileContent = File.ReadAllText(_filePath);

			var lines = (_securityService?.Decrypt(fileContent, GetLock()) ?? fileContent).Split(Environment.NewLine);

			_configuration.Parse(lines);

			return true;
		}

		/// <inheritdoc/>
		public bool Save()
		{
			var stringifiedConfig = string.Join(Environment.NewLine, _configuration.Stringify());

			var fileContent = _securityService?.Encrypt(stringifiedConfig, GetLock()) ?? stringifiedConfig;

			File.WriteAllText(_filePath, fileContent);

			return true;
		}

		private string GetLock()
			=> _filePath[((int)Math.Floor(_filePath.Length * 0.25))..((int)Math.Floor(_filePath.Length * 0.75))];
	}
}
