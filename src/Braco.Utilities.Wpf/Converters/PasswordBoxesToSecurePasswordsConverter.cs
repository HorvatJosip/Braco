using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting <see cref="PasswordBox"/>es into <see cref="SecureString"/>s. Basically,
	/// just for extracting the passwords from the <see cref="PasswordBox"/>es.
	/// </summary>
	public class PasswordBoxesToSecurePasswordsConverter : BaseMultiValueConverter<PasswordBoxesToSecurePasswordsConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if(values?.Length > 0)
			{
				var passwordBoxes = values.Select(pwBox => (PasswordBox)pwBox).ToList();

				return new Func<List<SecureString>>(() => passwordBoxes.Select(pwBox => pwBox.SecurePassword).ToList());
			}

			return null;
		}

		/// <inheritdoc/>
		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
