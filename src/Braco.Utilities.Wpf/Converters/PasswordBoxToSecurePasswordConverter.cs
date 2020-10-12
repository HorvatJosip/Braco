using System;
using System.Globalization;
using System.Security;
using System.Windows.Controls;
using System.Windows.Data;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Used for converting <see cref="PasswordBox"/> into <see cref="SecureString"/>. Basically,
    /// just for extracting the password from the <see cref="PasswordBox"/>.
    /// </summary>
    [ValueConversion(typeof(PasswordBox), typeof(SecureString))]
    public class PasswordBoxToSecurePasswordConverter : BaseConverter<PasswordBoxToSecurePasswordConverter>
    {
        /// <summary>
        /// If the <paramref name="value"/> is a <see cref="PasswordBox"/>,
        /// its secure password getter is returned.
        /// </summary>
        /// <param name="value">A <see cref="PasswordBox"/>.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns></returns>
		/// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PasswordBox passwordBox)
                return new Func<SecureString>(() => passwordBox.SecurePassword);

            return null;
        }

		/// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
