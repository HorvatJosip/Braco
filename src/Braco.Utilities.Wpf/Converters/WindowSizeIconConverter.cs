using System;
using System.Globalization;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for getting appropriate icon for window size.
	/// </summary>
    public class WindowSizeIconConverter : BaseConverter<WindowSizeIconConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is bool isMaximized)
			{
				return isMaximized ? ResourceKeys.RestoreIcon : ResourceKeys.MaximizeIcon;
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
