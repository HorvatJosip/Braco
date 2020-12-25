using System;
using System.Globalization;
using System.Windows.Media;

namespace Braco.Utilities.Wpf
{
	public class StringToBrushConverter : BaseConverter<StringToBrushConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is string colorString)
			{
				var converter = new BrushConverter();

				return converter.ConvertFromString(colorString);
			}

			return null;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
