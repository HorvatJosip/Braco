using System;
using System.Globalization;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting bool value to <see cref="Visibility"/>.
	/// </summary>
	public class BoolToVisibilityConverter : BaseConverter<BoolToVisibilityConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// If we received a boolean...
			if (value is bool visible)
				// Convert based on the given value
				return VisibilityHelpers.Convert(visible, parameter);

			// Otherwise, return collapsed
			return Visibility.Collapsed;
		}

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
