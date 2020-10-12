using System;
using System.Globalization;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting nullable object to <see cref="Visibility"/>.
	/// </summary>
	public class NullToVisibilityConverter : BaseConverter<NullToVisibilityConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			// Convert based on the given value
			=> VisibilityHelpers.Convert(value != null, parameter);

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
