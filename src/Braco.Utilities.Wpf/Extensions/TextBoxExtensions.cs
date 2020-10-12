using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Extensions
{
	/// <summary>
	/// Extensions for <see cref="TextBox"/>.
	/// </summary>
    public static class TextBoxExtensions
    {
		/// <summary>
		/// Sets the text inside the given <see cref="TextBox"/> and
		/// moves the cursor to the end.
		/// </summary>
		/// <param name="textBox"><see cref="TextBox"/> to use.</param>
		/// <param name="text">Text to set inside it.</param>
        public static void SetTextAndMoveCaretToEnd(this TextBox textBox, string text)
        {
            if (textBox != null)
            {
                textBox.Text = text;

                if (text != null)
                    textBox.CaretIndex = text.Length;
            }
        }
    }
}
