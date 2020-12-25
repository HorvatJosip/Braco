using Braco.Utilities.Wpf;
using System;
using System.Globalization;

namespace Braco.Generator
{
	public class RemoveFontFromFamilyConverter : BaseMultiValueConverter<RemoveFontFromFamilyConverter>
	{
		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if(values?.Length == 2 && values[0] is FontFamilyViewModel fontFamily && values[1] is FontViewModel font)
			{
				return new RemoveFontFromFamilyRequest(fontFamily, font);
			}

			return null;
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
