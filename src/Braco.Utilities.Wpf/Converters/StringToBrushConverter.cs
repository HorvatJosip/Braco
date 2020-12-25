using System;
using System.Globalization;
using System.Windows.Media;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Converts the given string to a resource using <see cref="BrushConverter"/>.
	/// </summary>
	public class StringToBrushConverter : BaseConverter<StringToBrushConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is string colorString)
			{
				var converter = new BrushConverter();

				return converter.ConvertFromString(colorString);
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
