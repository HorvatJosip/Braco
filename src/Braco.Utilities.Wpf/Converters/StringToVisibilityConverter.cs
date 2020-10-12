using Braco.Utilities.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting <see cref="string"/> into <see cref="Visibility"/>.
	/// <para>Checks a string if it's null or empty. If it is, <see cref="Visibility.Collapsed"/> is returned.
	/// Otherwise, <see cref="Visibility.Visible"/> is returned.</para>
	/// <para>In order to change this default behaviour, you can pass in a specific string as parameter.
	/// The capitalization doesn't matter.</para>
	/// <para>"hide" - this will return <see cref="Visibility.Hidden"/> instead of <see cref="Visibility.Collapsed"/>
	/// when there is no text in the string.</para>
	/// <para>"invert" - this will invert the behavior (when there is some text, it will return
	/// <see cref="Visibility.Collapsed"/>, but when there isn't any, it will return <see cref="Visibility.Visible"/>).</para>
	/// <para>"hide and invert" - this will combine the "hide" and "invert" options. So, when there is some text,
	/// it will return <see cref="Visibility.Hidden"/> and when there isn't any,
	/// it will return <see cref="Visibility.Visible"/>).</para>
	/// </summary>
	[ValueConversion(typeof(string), typeof(Visibility))]
    public class StringToVisibilityConverter : BaseConverter<StringToVisibilityConverter>
    {
		/// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If we received a string...
            if (value is string str)
            {
				// Convert based on string length
				return VisibilityHelpers.Convert(str.IsNotNullOrEmpty(), parameter);
			}

            // Otherwise, return collapsed
            return Visibility.Collapsed;
        }

        /// <exception cref="NotImplementedException"/>
		/// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
