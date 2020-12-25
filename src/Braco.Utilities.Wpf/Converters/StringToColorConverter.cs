using System;
using System.Globalization;
using System.Windows.Media;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Converts the given string to a color using <see cref="ColorConverter"/>.
	/// </summary>
	public class StringToColorConverter : BaseConverter<StringToColorConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is string colorString)
			{
				return ColorConverter.ConvertFromString(colorString);
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
