using System;
using System.Collections.Generic;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Defines logic of a localizer
	/// </summary>
	public interface ILocalizer
    {
        /// <summary>
        /// Current language culture.
        /// </summary>
        string Culture { get; }

        /// <summary>
        /// Fired when the <see cref="ChangeLanguage(string)"/>
        /// goes through successfully.
        /// </summary>
        event EventHandler<LanguageChangedEventArgs> LanguageChanged;

        /// <summary>
        /// Gets localized strings in all languages for the given key.
        /// </summary>
        /// <param name="key">Key used to get the localized strings.</param>
        /// <returns></returns>
        IList<string> GetAllValues(string key);

        /// <summary>
        /// Gets a localized string by key.
        /// </summary>
        /// <param name="key">Key used to get a localized string.</param>
        /// <returns>A localized string.</returns>
        string this[string key] { get; }

		/// <summary>
		/// Gets a localized string by key from specific section.
		/// </summary>
		/// <param name="sectionName">Name of the section to use.</param>
		/// <param name="key">Key used to get a localized string.</param>
		/// <returns>A localized string.</returns>
		string this[string sectionName, string key] { get; }

		/// <summary>
		/// Gets a localized string from an array at a specific index by key.
		/// </summary>
		/// <param name="key">Key of the array.</param>
		/// <param name="index">Index of the string in the array.</param>
		/// <returns>A localized string.</returns>
		string this[string key, int index] { get; }

		/// <summary>
		/// Gets a localized string from an array at a specific index
		/// by key from specific section.
		/// </summary>
		/// <param name="sectionName">Name of the section to use.</param>
		/// <param name="key">Key of the array.</param>
		/// <param name="index">Index of the string in the array.</param>
		/// <returns>A localized string.</returns>
		string this[string sectionName, string key, int index] { get; }

		/// <summary>
		/// Formats the localized string using the <see cref="string.Format(string, object[])"/> method.
		/// </summary>
		/// <param name="key">Key used to get a localized string.</param>
		/// <param name="parameters">Parameters that will be used to fill placeholders.</param>
		/// <returns>A localized formatted string.</returns>
		string Format(string key, IEnumerable<object> parameters);

		/// <summary>
		/// Formats the localized string using the <see cref="string.Format(string, object[])"/> method.
		/// </summary>
		/// <param name="sectionName">Name of the section to use.</param>
		/// <param name="key">Key used to get a localized string.</param>
		/// <param name="parameters">Parameters that will be used to fill placeholders.</param>
		/// <returns>A localized formatted string.</returns>
		string Format(string sectionName, string key, IEnumerable<object> parameters);

		/// <summary>
		/// Formats the localized string using the <see cref="string.Format(string, object[])"/> method.
		/// </summary>
		/// <param name="key">Key of the array.</param>
		/// <param name="index">Index of the string in the array.</param>
		/// <param name="parameters">Parameters that will be used to fill placeholders.</param>
		/// <returns>A localized formatted string.</returns>
		string Format(string key, int index, IEnumerable<object> parameters);

		/// <summary>
		/// Formats the localized string using the <see cref="string.Format(string, object[])"/> method.
		/// </summary>
		/// <param name="sectionName">Name of the section to use.</param>
		/// <param name="key">Key of the array.</param>
		/// <param name="index">Index of the string in the array.</param>
		/// <param name="parameters">Parameters that will be used to fill placeholders.</param>
		/// <returns>A localized formatted string.</returns>
		string Format(string sectionName, string key, int index, IEnumerable<object> parameters);

        /// <summary>
        /// Tries to change culture to the specified one.
        /// </summary>
        /// <param name="culture">Culture identifier (e.g. en-US).</param>
        /// <returns></returns>
        bool ChangeLanguage(string culture);
    }
}
