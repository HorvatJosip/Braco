using System;
using System.Globalization;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	public class ResourceKeyToResourceConverter : BaseConverter<ResourceKeyToResourceConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> Application.Current.TryFindResource(value);

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
