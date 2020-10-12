using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Implementation of <see cref="ILocalizer"/> using <see cref="ResourceDictionary"/>ies.
	/// </summary>
	public class WpfDictionaryLocalizer : ILocalizer
	{
        /// <summary>
        /// Used for identifying resource dictionaries used for localization.
        /// </summary>
		public const string Keyword = "Localization";

		private readonly List<ResourceDictionary> dictionaries = new List<ResourceDictionary>();
        private readonly string[] _cultures;

		/// <inheritdoc/>
		public string Culture { get; private set; }

		/// <inheritdoc/>
		public event EventHandler<LanguageChangedEventArgs> LanguageChanged;

		/// <summary>
		/// Creates an instance of the localizer.
		/// </summary>
		/// <param name="cultures">Cultures that are defined for the project.</param>
		public WpfDictionaryLocalizer(params string[] cultures)
		{
            _cultures = cultures ?? throw new ArgumentNullException(nameof(cultures));

			foreach (var dictionary in Application.Current.Resources.MergedDictionaries)
				if (dictionary.Source.OriginalString.Contains(Keyword))
					dictionaries.Add(dictionary);
		}

		/// <inheritdoc/>
		public bool ChangeLanguage(string culture)
		{
			if (culture == null || Culture == culture)
				return false;

			foreach (var dictionary in dictionaries)
			{
				var newUri = dictionary.NewUri(Keyword, culture);

				if (newUri == null)
					continue;

				try { dictionary.Source = newUri; }
				catch { return false; }
			}

			var cultureInfo = new CultureInfo(culture);
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			Culture = culture;
			LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(culture));
			return true;
		}

		private string GetItem(string key, string dictionary = null, int? index = null)
		{
            if (key.IsNullOrEmpty()) return null;

            string result = $"[{key}]";

			ResourceDictionary dic = null;

			if (dictionary != null)
				dic = dictionaries.Find(d => d.Source.OriginalString.Contains(dictionary));

			if (dic == null || !dic.Contains(key))
				foreach (var d in dictionaries)
					if (d.Contains(key))
					{
						dic = d;
						break;
					}

			if (dic == null)
				return null;

			var item = dic[key];

			if (index.HasValue)
				item = (item as string[])[index.Value];

            if (item != null)
                result = item.ToString();

            return result;
        }

		/// <inheritdoc/>
        public string this[string key] => GetItem(key);

		/// <inheritdoc/>
		public string this[string dictionaryName, string key] => GetItem(key, dictionaryName);

		/// <inheritdoc/>
		public string this[string dictionaryName, string key, int index] => GetItem(key, dictionaryName, index);

		/// <inheritdoc/>
		public string this[string key, int index] => GetItem(key, null, index);

		/// <inheritdoc/>
		public string Format(string key, IEnumerable<object> parameters)
			=> string.Format(format: this[key], args: parameters.ToArray());

		/// <inheritdoc/>
		public string Format(string dictionaryName, string key, IEnumerable<object> parameters)
			=> string.Format(format: this[dictionaryName, key], args: parameters.ToArray());

		/// <inheritdoc/>
		public string Format(string key, int index, IEnumerable<object> parameters)
			=> string.Format(format: this[key, index], args: parameters.ToArray());

		/// <inheritdoc/>
		public string Format(string dictionaryName, string key, int index, IEnumerable<object> parameters)
			=> string.Format(format: this[dictionaryName, key, index], args: parameters.ToArray());

		/// <inheritdoc/>
		public IList<string> GetAllValues(string key)
		{
			var values = new List<string>();

			foreach (var culture in _cultures)
			{
				if (culture != Culture)
					foreach (var dictionary in dictionaries)
					{
						var temp = new ResourceDictionary { Source = dictionary.NewUri(Keyword, culture) };

						if (temp.Contains(key) && temp[key] is string value)
						{
							values.Add(value);
							break;
						}
					}
				else
					foreach (var dictionary in dictionaries)
						if (dictionary.Contains(key) && dictionary[key] is string value)
						{
							values.Add(value);
							break;
						}
			}

			return values;
		}
	}
}
