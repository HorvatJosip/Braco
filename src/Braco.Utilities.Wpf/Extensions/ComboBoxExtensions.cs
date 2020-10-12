using Braco.Utilities.Extensions;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Extensions
{
	/// <summary>
	/// Extension used for <see cref="ComboBox"/>es.
	/// </summary>
    public static class ComboBoxExtensions
    {
		/// <summary>
		/// Extracts text out of the <see cref="ComboBox"/> item.
		/// </summary>
		/// <param name="comboBox"><see cref="ComboBox"/> to get the text from.</param>
		/// <param name="item">Item that contains the text.</param>
		/// <returns>Text of the item in the <see cref="ComboBox"/>.</returns>
        public static string GetItemText(this ComboBox comboBox, object item)
            => comboBox.DisplayMemberPath.IsNullOrWhiteSpace()
                ? (string)item
                : (string)item.GetType().GetProperty(comboBox.DisplayMemberPath).GetValue(item);

		/// <summary>
		/// Gets text from the selected item in the <see cref="ComboBox"/>.
		/// </summary>
		/// <param name="comboBox"><see cref="ComboBox"/> to get the text from.</param>
		/// <returns>Text of the selected item in the <see cref="ComboBox"/>.</returns>
		public static string GetSelectedItemText(this ComboBox comboBox)
            => comboBox.GetItemText(comboBox.SelectedItem);
    }
}
