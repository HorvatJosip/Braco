using Braco.Utilities.Extensions;
using System;
using System.Globalization;
using System.Linq;

namespace Braco.Utilities.Wpf
{
	public class RemainingSizeCalculationConverter : BaseMultiValueConverter<RemainingSizeCalculationConverter>
	{
		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.IsNullOrEmpty()) return 0;

			var numberValues = values.Where(x => x is double).Select(x => (double)x).ToList();

			if(numberValues.Count > 0 && (parameter is double anotherValue || double.TryParse(parameter?.ToString(), out anotherValue)))
			{
				numberValues.Add(anotherValue);
			}

			return numberValues.Count switch
			{
				1 => numberValues[0],
				> 1 => numberValues[0] - numberValues.Skip(1).Sum(),
				_ => 0
			};
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
