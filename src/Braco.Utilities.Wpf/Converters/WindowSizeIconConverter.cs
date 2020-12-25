using System;
using System.Globalization;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for getting appropriate icon for window size.
	/// </summary>
    public class WindowSizeIconConverter : BaseConverter<WindowSizeIconConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value switch
			{
				bool isMaximized => isMaximized ? ResourceKeys.RestoreIcon : ResourceKeys.MaximizeIcon,
				WindowState windowState => windowState == WindowState.Maximized ? ResourceKeys.RestoreIcon : ResourceKeys.MaximizeIcon,
				_ => null
			};

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
