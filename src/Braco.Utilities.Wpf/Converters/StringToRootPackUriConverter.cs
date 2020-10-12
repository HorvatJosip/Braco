using System;
using System.Globalization;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting a <see cref="string"/> into root pack uri.
	/// </summary>
	public class StringToRootPackUriConverter : BaseConverter<StringToRootPackUriConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string str)
			{
				return new Uri(PackUtilities.GetRootPackUriWithSuffix(str));
			}

			return null;
		}

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
