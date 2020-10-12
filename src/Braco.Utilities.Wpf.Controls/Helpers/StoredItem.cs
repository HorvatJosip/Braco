using Braco.Utilities.Wpf.Extensions;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Helper for working with toggle button (part of combo box) control hierarchy.
	/// </summary>
	/// <typeparam name="T">Type of value to store.</typeparam>
	public class StoredItem<T>
	{
		/// <summary>
		/// Arrow button used for opening the <see cref="ComboBox"/>.
		/// </summary>
		public ImageButton DownArrow { get; set; }

		/// <summary>
		/// Toggle button for the <see cref="ComboBox"/>.
		/// </summary>
		public ToggleButton ToggleButton { get; set; }

		/// <summary>
		/// ComboBox.
		/// </summary>
		public ComboBox ComboBox { get; set; }

		/// <summary>
		/// Editable part of <see cref="ComboBox"/>, if available.
		/// </summary>
		public TextBox TextBox { get; set; }

		/// <summary>
		/// Value of the attached property.
		/// </summary>
		public T Value { get; set; }

		/// <summary>
		/// Text of the <see cref="TextBox"/>.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Last selected index of the <see cref="ComboBox"/>.
		/// </summary>
		public int? LastSelectedIndex { get; set; }

		/// <summary>
		/// Creates the item.
		/// </summary>
		/// <param name="toggleButton">Toggle button for the <see cref="ComboBox"/>.</param>
		public StoredItem(ToggleButton toggleButton) => ToggleButton = toggleButton;

		/// <summary>
		/// Creates the item.
		/// </summary>
		/// <param name="toggleButton">Toggle button for the <see cref="ComboBox"/>.</param>
		/// <param name="downArrow">Arrow button used for opening the <see cref="ComboBox"/>.</param>
		/// <param name="comboBox">ComboBox.</param>
		/// <param name="textBox">Editable part of <see cref="ComboBox"/>, if available.</param>
		/// <param name="value">Value of the attached property.</param>
		public StoredItem(ToggleButton toggleButton, ImageButton downArrow, ComboBox comboBox, TextBox textBox, T value) : this(toggleButton)
			=> SetupRest(downArrow, comboBox, textBox, value);

		/// <summary>
		/// Creates the item.
		/// </summary>
		/// <param name="toggleButton">Toggle button for the <see cref="ComboBox"/>.</param>
		/// <param name="downArrow">Arrow button used for opening the <see cref="ComboBox"/>.</param>
		/// <param name="comboBox">ComboBox.</param>
		/// <param name="value">Value of the attached property.</param>
		public StoredItem(ToggleButton toggleButton, ImageButton downArrow, ComboBox comboBox, T value) : this(toggleButton)
			=> SetupRest(downArrow, comboBox, value);

		/// <summary>
		/// Sets up the rest of the properties.
		/// </summary>
		/// <param name="downArrow">Arrow button used for opening the <see cref="ComboBox"/>.</param>
		/// <param name="comboBox">ComboBox.</param>
		/// <param name="textBox">Editable part of <see cref="ComboBox"/>, if available.</param>
		/// <param name="value">Value of the attached property.</param>
		public void SetupRest(ImageButton downArrow, ComboBox comboBox, TextBox textBox, T value)
		{
			TextBox = textBox;
			SetupRest(downArrow, comboBox, value);
		}

		/// <summary>
		/// Sets up the rest of the properties.
		/// </summary>
		/// <param name="downArrow">Arrow button used for opening the <see cref="ComboBox"/>.</param>
		/// <param name="comboBox">ComboBox.</param>
		/// <param name="value">Value of the attached property.</param>
		public void SetupRest(ImageButton downArrow, ComboBox comboBox, T value)
		{
			DownArrow = downArrow;
			ComboBox = comboBox;
			Value = value;
		}

		/// <summary>
		/// Closes the <see cref="ComboBox"/>, sets the text of
		/// the <see cref="TextBox"/> to <paramref name="text"/> and
		/// moves the cursor to the end of the <see cref="TextBox"/>.
		/// </summary>
		/// <param name="text">Text to input into <see cref="TextBox"/>.</param>
		public void CloseComboBoxAndShowText(string text)
		{
			ComboBox.IsDropDownOpen = false;

			TextBox.SetTextAndMoveCaretToEnd(text);

			LastSelectedIndex = null;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"{Text}: {Value} (last selected index: {LastSelectedIndex})";
	}
}
