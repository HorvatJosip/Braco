using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Fetches appropriate toggle button style for the <see cref="ComboBox"/>.
	/// </summary>
	public class ComboBoxToggleButtonStyleConverter : BaseConverter<ComboBoxToggleButtonStyleConverter>
	{
		/// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is bool isEditable)
			{
				return (Style)Application.Current.FindResource(
					isEditable 
						? ResourceKeys.EditableComboBoxToggleButtonStyle
                        : ResourceKeys.ReadOnlyComboBoxToggleButtonStyle
                );
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
