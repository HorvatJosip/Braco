using System;
using System.Globalization;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting a bool to its opposite state.
	/// </summary>
	public class InverseBoolConverter : BaseConverter<InverseBoolConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolean) return !boolean;

			return null;
		}

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
