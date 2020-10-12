using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Used for <see cref="ItemsControl"/> to specify its <see cref="ItemsControl.ItemsPanel"/>
    /// through binding. The result depends on the binding on boolean property - if the
    /// property returns true, <see cref="StackPanel"/> will be the panel for the items. If
    /// it returns false, <see cref="WrapPanel"/> will be the panel for the items.
    /// </summary>
	public class ItemsPanelConverter : BaseConverter<ItemsPanelConverter>
	{
		private const string itemsPanelTemplate = @"
			<ItemsPanelTemplate 
				xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
				xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
					{0}
			</ItemsPanelTemplate>
";

		private const string stackPanel = @"
			<StackPanel HorizontalAlignment=""Center"" />
";

		private const string wrapPanel = @"
			<WrapPanel HorizontalAlignment=""Right"" />
";

		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool stack)
			{
				var panelTemplate = string.Format(itemsPanelTemplate, stack ? stackPanel : wrapPanel);

				return XamlReader.Parse(panelTemplate) as ItemsPanelTemplate;
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
