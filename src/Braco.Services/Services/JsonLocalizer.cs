using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Braco.Services
{
	/// <summary>
	/// Implementation of <see cref="ILocalizer"/> using JSON files.
	/// </summary>
    public class JsonLocalizer : ILocalizer
    {
		/// <summary>
		/// Key for the folder that will be used.
		/// </summary>
        public const string FolderKey = "Locales";
		/// <summary>
		/// Name of the folder that will be used.
		/// </summary>
        public const string FolderName = "Localization";

        private readonly DirectoryInfo _localesDir;
        private readonly Dictionary<string, JObject> _localesByCulture;

        private JObject _currentLocales;

		/// <inheritdoc/>
        public string Culture { get; private set; }

		/// <inheritdoc/>
        public event EventHandler<LanguageChangedEventArgs> LanguageChanged;

		/// <summary>
		/// Creates an instance of the localizer.
		/// </summary>
		/// <param name="currentCulture">Culture that is currently active.</param>
		/// <param name="cultures">Possible cultures.</param>
        public JsonLocalizer(IPathManager pathManager, string currentCulture, params string[] cultures)
        {
            if(!IsCultureValid(currentCulture)) 
                throw new ArgumentException($"{currentCulture} is not a valid culture.", nameof(currentCulture));
            if (cultures == null) throw new ArgumentNullException(nameof(cultures));
            if (!currentCulture.In(cultures))
                throw new ArgumentException("Current culture must be part of given cultures", nameof(currentCulture));

			_localesDir = pathManager.AddDirectory(FolderKey, Path.Combine(pathManager.AppDirectory.FullName, FolderName));

            _localesByCulture = new Dictionary<string, JObject>();
            cultures.ToHashSet().ForEach(culture =>
            {
                if(!IsCultureValid(culture))
                    throw new ArgumentException($"{culture} is not a valid culture.", nameof(cultures));

                var file = pathManager.AddFileToDirectory(culture, _localesDir, $"{culture}.json");
                _localesByCulture.Add(culture, JObject.Parse(File.ReadAllText(file.FullName)));
            });

            _currentLocales = _localesByCulture[currentCulture];
        }

		/// <inheritdoc/>
        public bool ChangeLanguage(string culture)
        {
            if (!IsCultureValid(culture))
                throw new ArgumentException($"{culture} is not a valid culture.", nameof(culture));

            if (!_localesByCulture.ContainsKey(culture))
                throw new ArgumentException($"{culture} doesn't exist among the locales.");

            if (Culture == culture) return false;

            _currentLocales = _localesByCulture[culture];

            var cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            Culture = culture;
            LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(culture));

            return true;
        }

		/// <inheritdoc/>
        public IList<string> GetAllValues(string key)
            => _localesByCulture
                .Select(kvp => kvp.Value[key].Value<string>())
                .ToList();

        private string GetItem(string key, string section = null, int? index = null)
        {
            if (key.IsNullOrEmpty()) return null;

            string result = $"[{key}]";

            var token = section != null ? _currentLocales[section] : _currentLocales;

            if (index.HasValue)
            {
                var array = token[key]?.Value<string[]>();

                if (array != null && array.Length < index.Value)
                    result = array[index.Value];
            }
            else
            {
                var item = token[key];

                if(item != null)
                    result = item?.Value<string>();
            }

            return result;
        }

		/// <inheritdoc/>
        public string this[string sectionName, string key, int index] => GetItem(key, sectionName, index);

		/// <inheritdoc/>
        public string this[string key, int index] => GetItem(key, null, index);

		/// <inheritdoc/>
        public string this[string sectionName, string key] => GetItem(key, sectionName);

		/// <inheritdoc/>
        public string this[string key] => GetItem(key);

		/// <inheritdoc/>
        public string Format(string key, IEnumerable<object> parameters)
            => string.Format(format: this[key], args: parameters.ToArray());

		/// <inheritdoc/>
        public string Format(string sectionName, string key, IEnumerable<object> parameters)
            => string.Format(format: this[sectionName, key], args: parameters.ToArray());

		/// <inheritdoc/>
        public string Format(string key, int index, IEnumerable<object> parameters)
            => string.Format(format: this[key, index], args: parameters.ToArray());

		/// <inheritdoc/>
        public string Format(string sectionName, string key, int index, IEnumerable<object> parameters)
            => string.Format(format: this[sectionName, key, index], args: parameters.ToArray());

        private bool IsCultureValid(string culture)
        {
            if (culture.IsNullOrEmpty()) return false;

            return CultureInfo.GetCultureInfo(culture) != null;
        }
    }
}
