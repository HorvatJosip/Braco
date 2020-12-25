using System;
using System.Globalization;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting nullable object to <see cref="bool"/> value.
	/// </summary>
	public class NullToBoolConverter : BaseConverter<NullToBoolConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value != null;

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
