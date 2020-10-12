using System;
using System.Globalization;
using System.Linq;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Used for fetching file name from getter of the <see cref="ImageButton"/>.
	/// </summary>
	public class ImageButtonConverter : BaseMultiValueConverter<ImageButtonConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values?.Count() > 0 && values[0] is ImageButton imageButton)
			{
				if (imageButton.FileName == null)
					imageButton.FetchFileNameFromGetter(values.Skip(1).ToArray());

				return imageButton.GetImage();
			}

			return null;
		}

		/// <inheritdoc/>
		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
