using System;
using System.Windows;

namespace Braco.Utilities.Wpf.Extensions
{
	/// <summary>
	/// Extensions for <see cref="ResourceDictionary"/>ies.
	/// </summary>
    public static class ResourceDictionaryExtensions
    {
        /// <summary>
        /// Gets a new uri for the dictionary after the keyword.
        /// </summary>
        /// <param name="dictionary">Dictionary from which to extract the current uri.</param>
        /// <param name="keyword">Keyword to look for.</param>
        /// <param name="newValue">Value used to replace the part after the keyword.</param>
        /// <returns></returns>
        public static Uri NewUri(this ResourceDictionary dictionary, string keyword, string newValue)
        {
            var uri = dictionary.Source;

            var keywordIndex = uri.OriginalString.IndexOf(keyword);
            var left = uri.OriginalString.IndexOf('/', keywordIndex) + 1;
            var right = uri.OriginalString.IndexOf('/', left);

            var currentValue = uri.OriginalString[left..right];

            if (currentValue == newValue)
                return null;

            return uri.IsAbsoluteUri
                ? new Uri(uri.AbsolutePath.Replace(currentValue, newValue))
                : new Uri(string.Format("{0}{1}{2}",
                    uri.OriginalString.Substring(0, left),
                    newValue,
                    uri.OriginalString.Substring(right)
                ), UriKind.Relative);
        }
    }
}
