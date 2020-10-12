using Braco.Utilities.Wpf.Extensions;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for extracting text of the item from the <see cref="ComboBox"/>
	/// </summary>
	public class ComboBoxItemConverter : BaseMultiValueConverter<ComboBoxItemConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if(values?.Length >= 2 && values[0] != null && values[1] is ComboBox comboBox)
				return comboBox.GetItemText(values[0]);

			return null;
		}

		/// <inheritdoc/>
		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
