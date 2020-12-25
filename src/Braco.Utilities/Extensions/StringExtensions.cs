using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Braco.Utilities.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="string"/>.
	/// </summary>
	public static class StringExtensions
    {
		private const int defaultFuzzySearchLimit = 3;

        /// <summary>
        /// Indicates whether the specified string is null or a <see cref="string.Empty"/> string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
            => string.IsNullOrEmpty(value);

		/// <summary>
		/// Indicates whether the specified string is not null or a <see cref="string.Empty"/> string.
		/// </summary>
		/// <param name="value">The string to test.</param>
		/// <returns></returns>
		public static bool IsNotNullOrEmpty(this string value)
			=> !IsNullOrEmpty(value);

        /// <summary>
        /// Indicates whether a specified string is null, empty,
        /// or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string value)
            => string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Indicates whether a specified string is not null, empty,
        /// or doesn't consist only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns></returns>
        public static bool IsNotNullOrWhiteSpace(this string value)
			=> !IsNullOrWhiteSpace(value);

        /// <summary>
        /// If the string is null, <paramref name="newValue"/> is returned.
        /// </summary>
        /// <param name="value">Current value.</param>
        /// <param name="newValue">Value to return if the <paramref name="value"/> is null.</param>
        /// <returns></returns>
        public static string ReplaceIfNull(this string value, string newValue)
            => value ?? newValue;

        /// <summary>
        /// If the string is null or empty, <paramref name="newValue"/> is returned.
        /// </summary>
        /// <param name="value">Current value.</param>
        /// <param name="newValue">Value to return if the <paramref name="value"/> is null or empty.</param>
        /// <returns></returns>
        public static string ReplaceIfNullOrEmpty(this string value, string newValue)
            => value.IsNullOrEmpty() ? newValue : value;

        /// <summary>
        /// If the string is null or whiteSpace, <paramref name="newValue"/> is returned.
        /// </summary>
        /// <param name="value">Current value.</param>
        /// <param name="newValue">Value to return if the <paramref name="value"/> is null or whiteSpace.</param>
        /// <returns></returns>
        public static string ReplaceIfNullOrWhiteSpace(this string value, string newValue)
            => value.IsNullOrWhiteSpace() ? newValue : value;

        /// <summary>
        /// Converts the given value to base64 encoded version.
        /// </summary>
        /// <param name="value">Value to convert to base64.</param>
        /// <returns>base64 encoded version of <paramref name="value"/>.</returns>
        public static string ToBase64(this string value)
            => System.Convert.ToBase64String(Encoding.UTF8.GetBytes(value ?? ""));

        /// <summary>
        /// Converts the given value from base64 encoded version.
        /// </summary>
        /// <param name="value">Value that is base64 encoded.</param>
        /// <returns>Decoded version of <paramref name="value"/> that was previously base64 encoded.</returns>
        public static string FromBase64(this string value)
            => Encoding.UTF8.GetString(System.Convert.FromBase64String(value ?? ""));

        /// <summary>
        /// Takes in a string and surrounds it with two other strings.
        /// </summary>
        /// <param name="value">Current string.</param>
        /// <param name="before">String to prepend to the current string.</param>
        /// <param name="after">String to append to the current string.</param>
        /// <returns>String with <paramref name="before"/> prepended and <paramref name="after"/> appended.</returns>
        public static string SurroundWith(this string value, string before, string after)
            => $"{before}{value}{after}";

		/// <summary>
		/// Takes in a string and surrounds it with two strings of same value.
		/// </summary>
		/// <param name="value">Current string.</param>
		/// <param name="beforeAndAfter">String to prepend and append to the current string.</param>
		/// <returns>String with <paramref name="beforeAndAfter"/> prepended and appended.</returns>
		public static string SurroundWith(this string value, string beforeAndAfter)
			=> value.SurroundWith(beforeAndAfter, beforeAndAfter);

        /// <summary>
        /// Removes all of the whitespace from a string.
        /// </summary>
        /// <param name="value">String to remove the whitespace from.</param>
        /// <returns></returns>
        public static string WithoutWhiteSpace(this string value) => new string
        (
            value?.Where(@char => !char.IsWhiteSpace(@char)).ToArray()
        );

        /// <summary>
        /// Removes all of the characters given in <paramref name="charactersToRemove"/>.
        /// </summary>
        /// <param name="value">Value from which to remove the characters.</param>
        /// <param name="charactersToRemove">Characters to remove from <paramref name="value"/>.
        /// <para>Note: you can pass in a <see cref="string"/> because it implements <see cref="IEnumerable"/>&lt;<see cref="char"/>&gt;.</para></param>
        /// <returns><paramref name="value"/> without <paramref name="charactersToRemove"/>.</returns>
        public static string Without(this string value, IEnumerable<char> charactersToRemove) => new string
        (
            value?.Where(@char => !charactersToRemove.Contains(@char)).ToArray()
        );

        /// <summary>
        /// Finds how many times does a given "target" substring occurr
        /// in the current string.
        /// </summary>
        /// <param name="value">String to search.</param>
        /// <param name="target">Substring to find the count of.</param>
        /// <returns></returns>
        public static int SubstringCount(this string value, string target)
        {
            if (value == null) throw new NullReferenceException(nameof(value));
            if (target == null) throw new ArgumentException(nameof(target));

            // Initialize the counter
            var count = 0;
            // Setup the current index
            int currentIndex = -target.Length;

            // Loop until the target value is no longer found
            while (true)
            {
                // Try to find the next occurence of the target string
                currentIndex = value.IndexOf(target, currentIndex + target.Length);

                // If the substring was found...
                if (currentIndex != -1)
                    // Increase the counter
                    count++;

                // Otherwise...
                else
                    // Return the substring count
                    return count;
            }
        }

        /// <summary>
        /// Converts a string into an object of the specified type.
        /// </summary>
        /// <param name="value">String to convert into an object.</param>
        /// <param name="type">Type of object to convert the string into.</param>
        /// <param name="culture">Culture used for converting to string.</param>
        /// <param name="context">Context info about the <see cref="TypeDescriptor"/> for the type.</param>
        /// <returns></returns>
        public static object Convert(this string value, Type type, CultureInfo culture = null, ITypeDescriptorContext context = null)
        {
            // Get the type converter
            var typeConverter = TypeDescriptor.GetConverter(type);

            // Try to convert from string to the wanted type
            return typeConverter.ConvertFromString(context, culture, value);
        }

        /// <summary>
        /// Converts a string into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to convert the string into.</typeparam>
        /// <param name="value">String to convert into an object.</param>
        /// <param name="culture">Culture used for converting to string.</param>
        /// <param name="context">Context info about the <see cref="TypeDescriptor"/> for the type <typeparamref name="T"/>.</param>
        /// <returns></returns>
        public static T Convert<T>(this string value, CultureInfo culture = null, ITypeDescriptorContext context = null)
            => (T)Convert(value, typeof(T), culture, context);

        /// <summary>
        /// Replaces whitespace inside a string with a given string.
        /// </summary>
        /// <param name="value">String to replace the whitespace in.</param>
        /// <param name="trim">Should the given string also be trimmed before replacement?</param>
        /// <param name="with">Replacement for whitespace.</param>
        /// <returns></returns>
        public static string ReplaceWhiteSpace(this string value, bool trim = true, string with = " ")
        {
            if (value.IsNullOrEmpty() || with.IsNullOrEmpty()) return value;

            return Regex.Replace(trim ? value.Trim() : value, "\\s+", with);
        }

		/// <summary>
		/// Formats a string using the given parameters.
		/// </summary>
		/// <param name="format">Format to use for the output.</param>
		/// <param name="formatParameters">Parameters to use to replace the format templates.</param>
		/// <returns>A string formatted using the given parameters.</returns>
		public static string Format(this string format, params object[] formatParameters)
			=> string.Format(format, formatParameters);

        /// <summary>
        /// Creates a secure string out of the given one.
        /// </summary>
        /// <param name="str">String to secure.</param>
        public static SecureString Secure(this string str)
        {
            var secureString = new SecureString();

            foreach (var character in str)
                secureString.AppendChar(character);

            return secureString;
        }

        /// <summary>
        /// Performs a partial search using the search query on each of the given items.
        /// </summary>
        /// <param name="search">Query to check against.</param>
        /// <param name="comparison">Comparison to use.</param>
        /// <param name="testValues">Values to check the query against.</param>
        /// <returns>True if some of the values partially match the search query.</returns>
        public static bool PartialSearch(this string search, StringComparison comparison, params string[] testValues)
        {
            if (search == null)
                return true;

            if (testValues.IsNullOrEmpty())
                return false;

            foreach (var value in testValues)
            {
                if (value == null)
                    continue;

                if (value.IndexOf(search, comparison) != -1)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Performs a partial search using the search query on each of the given items
        /// with <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
        /// </summary>
        /// <param name="search">Query to check against.</param>
        /// <param name="testValues">Values to check the query against.</param>
        /// <returns>True if some of the values partially match the search query.</returns>
        public static bool PartialSearch(this string search, params string[] testValues)
            => PartialSearch(search, StringComparison.InvariantCultureIgnoreCase, testValues);

        /// <summary>
        /// Performs fuzzy search on the given string.
        /// </summary>
        /// <param name="search">Search term to check.</param>
        /// <param name="maxDiff">Maximum character difference in a compared word.</param>
        /// <param name="testValues">Values to test.</param>
        /// <returns>If the search term matched some of the test values.</returns>
        public static bool FuzzySearch(this string search, int maxDiff, params string[] testValues)
        {
            if (search == null)
                return true;

            if (testValues.IsNullOrEmpty())
                return false;

            foreach (var value in testValues)
            {
                if (value == null)
                    continue;

                var diff = 0;

                for (int i = 0; i < Math.Min(search.Length, value.Length); i++)
                    if (search[i] != value[i])
                        diff++;

                if (diff + Math.Abs(search.Length - value.Length) <= maxDiff)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Performs fuzzy search on the given string.
        /// </summary>
        /// <param name="search">Search term to check.</param>
        /// <param name="testValues">Values to test.</param>
        /// <returns>If the search term matched some of the test values.</returns>
        public static bool FuzzySearch(this string search, params string[] testValues)
            => FuzzySearch(search, defaultFuzzySearchLimit, testValues);

        /// <summary>
        /// Takes the formatted string in with its format and replaces all
        /// of the fixed parts that are defined by the format.
        /// </summary>
        /// <param name="value">String to unformat.</param>
        /// <param name="format">Format that the string was formatted by.</param>
        /// <returns></returns>
        public static string Unformat(this string value, string format)
        {
            if (value == null) throw new NullReferenceException(nameof(value));
            if (format == null) throw new ArgumentException(nameof(format));

            // Create a collection that will store constant strings - the ones to remove
            var strings = new List<string>();
            // Declare the current working index and left brace index
            int index = 0, leftBrace;

            void AddString(bool full = false)
            {
                // If the string should be substringed until the end..
                string substring = full
                    // Substring from the index
                    ? format.Substring(index)
                    // Otherwise substring from the index and until the left brace
                    : format.Substring(index, leftBrace - index);

                // If it exists...
                if (substring.IsNotNullOrEmpty())
                    // Add it to the replacables
                    strings.Add(substring.Replace("}}", "}"));
            }

            // Loop until the format's length is hit
            while (index < format.Length)
            {
                // Get the index of the '{'
                leftBrace = format.IndexOf('{', index);

                // If the left brace is found...
                if (leftBrace != -1)
                {
                    // If the opening brace is the end of the format 
                    // or immediately followed by the closing brace...
                    if (leftBrace + 1 == format.Length || format[leftBrace + 1] == '}')
                        // Throw the format exception
                        throw new FormatException();

                    // Else if the next character is another opening brace...
                    else if (format[leftBrace + 1] == '{')
                    {
                        // Include the first brace
                        leftBrace += 1;

                        // Add string before the second brace
                        AddString();

                        // Skip the second brace
                        index = leftBrace + 1;
                    }

                    // Otherwise...
                    else
                    {
                        // Loop from that point to the end
                        for (int i = leftBrace + 1; i < format.Length; i++)
                        {
                            // If we've hit the closing brace...
                            if (format[i] == '}')
                            {
                                // Add the string until the left brace
                                AddString();

                                // Set the next starting point right after the closing brace
                                index = i + 1;

                                // Break out of for loop
                                break;
                            }

                            // Else if the character isn't a digit...
                            else if (!char.IsDigit(format[i]))
                                // Throw the format exception
                                throw new FormatException();
                        } // ENDOF: for
                    } // ENDOF: else
                } // ENDOF: if

                // Otherwise...
                else
                {
                    // Add the remainder of the string
                    AddString(true);

                    // Finish with the algorithm
                    break;
                }
            }

            // Foreach string that needs to be removed...
            foreach (var str in strings)
            {
                // Find its location
                var startIndex = value.IndexOf(str);

                // Remove it
                value = value.Remove(startIndex, str.Length);
            }

            // Remove the unformatted string
            return value;
        }
    }
}
