using System;
using System.Globalization;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for getting a brush based on info box type.
	/// </summary>
	public class InfoBoxTypeToBrushConverter : BaseConverter<InfoBoxTypeToBrushConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is InfoBoxType type)
			{
				return Application.Current.FindResource($"{nameof(InfoBoxType)}_{type}_Brush");
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
