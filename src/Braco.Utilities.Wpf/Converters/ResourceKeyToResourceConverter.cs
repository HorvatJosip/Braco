using System;
using System.Globalization;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting the given value to a resource (if found).
	/// </summary>
	public class ResourceKeyToResourceConverter : BaseConverter<ResourceKeyToResourceConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> Application.Current.TryFindResource(value);

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
