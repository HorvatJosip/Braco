using Braco.Utilities.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting a <see cref="string"/> or an object's
	/// string representation into <see cref="bool" />
	/// </summary>
	[ValueConversion(typeof(string), typeof(bool))]
    public class StringToBoolConverter : BaseConverter<StringToBoolConverter>
    {
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value?.ToString().IsNotNullOrEmpty();

        /// <exception cref="NotImplementedException"/>
		/// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
