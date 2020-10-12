using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf.Extensions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Used for setting up down arrow part of the <see cref="ComboBox"/>
	/// to open it on mouse up.
	/// </summary>
	public class SetupOnClickProperty : BaseAttachedProperty<SetupOnClickProperty, bool>
	{
		private readonly ToggleButtonAttachedPropertiesHelper<bool> _helper;

		/// <summary>
		/// Creates an instance of the attached property.
		/// </summary>
		public SetupOnClickProperty() => _helper = new ToggleButtonAttachedPropertiesHelper<bool>(false, OnSetup);

		private void OnSetup(StoredItem<bool> item)
		{
			// Remove any previous event handlers
			item.DownArrow.PreviewMouseUp -= DownArrow_PreviewMouseUp;

			// If the textbox should be key reactive...
			if (GetValue(item.ToggleButton))
			{
				// Start listening for clicks
				item.DownArrow.PreviewMouseUp += DownArrow_PreviewMouseUp;
			}
		}

		private void DownArrow_PreviewMouseUp(object sender, RoutedEventArgs e)
		{
			// Get the saved combobox
			var comboBox = _helper[sender];

			// Change its open state
			comboBox.IsDropDownOpen = !comboBox.IsDropDownOpen;

			// Look for a text box (in case there is the editable part)
			var textBox = ControlTree.FindChild<TextBox>(comboBox);

			// If it exists and the drop down was opened...
			if (textBox != null && comboBox.IsDropDownOpen)
				// Focus it
				textBox.Focus();
		}

		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
			=> _helper.OnValueChanged(sender, e);
	}

	/// <summary>
	/// Makes the <see cref="ComboBox"/> open on <see cref="TextBox"/>'s Focus event and allows for collection
	/// traversal using up and down keys. If the combo box is open, enter will
	/// close it, ot if it isn't, down key will open it.
	/// </summary>
	public class KeyReactiveProperty : BaseAttachedProperty<KeyReactiveProperty, bool>
	{
		private readonly ToggleButtonAttachedPropertiesHelper<bool> _helper;

		/// <summary>
		/// Creates an instance of the attached property.
		/// </summary>
		public KeyReactiveProperty() => _helper = new ToggleButtonAttachedPropertiesHelper<bool>(true, OnSetup);

		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
			=> _helper.OnValueChanged(sender, e);

		private void OnSetup(StoredItem<bool> item)
		{
			// Remove any previous event handlers
			item.TextBox.KeyUp -= TextBox_KeyUp;
			item.TextBox.KeyDown -= TextBox_KeyDown;
			item.TextBox.GotFocus -= TextBox_GotFocus;

			// If the textbox should be key reactive...
			if (item.Value)
			{
				// Start listening for key presses
				item.TextBox.KeyUp += TextBox_KeyUp;
				item.TextBox.KeyDown += TextBox_KeyDown;

				// Start listening for focus changes
				item.TextBox.GotFocus += TextBox_GotFocus;
			}
		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			// Get the saved combobox
			var comboBox = _helper[sender];

			// Open it on focus
			comboBox.IsDropDownOpen = true;
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			// Get the item
			var item = _helper.Find(sender);

			// Get the combobox
			var comboBox = item.ComboBox;

			// If the combobox is open...
			if (comboBox.IsDropDownOpen)
			{
				// Check which key was pressed
				switch (e.Key)
				{
					case Key.Enter:
						// If there are no items...
						if (comboBox.ItemsSource.IsNullOrEmpty())
							// Construct 
							LostFocusFillerProperty.Construct(comboBox);

						// Otherwise...
						else
							// Close it and show its text in the textbox
							_helper.Find(comboBox).CloseComboBoxAndShowText(comboBox.Text);
						break;

					case Key.Escape:
						// If something else was selected before...
						if (item.LastSelectedIndex.HasValue)
							// Return the selection to it
							comboBox.SelectedIndex = item.LastSelectedIndex.Value;

						// Reset the mechanism
						item.LastSelectedIndex = null;

						// Close it
						comboBox.IsDropDownOpen = false;
						break;
				}
			}
		}

		private void TextBox_KeyUp(object sender, KeyEventArgs e)
		{
			// Get the item
			var item = _helper.Find(sender);

			// Get the combobox
			var comboBox = item.ComboBox;

			// If the combobox is open...
			if (comboBox.IsDropDownOpen)
			{
				// Check which key was pressed
				switch (e.Key)
				{
					case Key.Up:
						// If the index isn't memorized yet...
						if (item.LastSelectedIndex == null)
							// Do it now
							item.LastSelectedIndex = comboBox.SelectedIndex;

						// If there are items above, go up
						if (comboBox.SelectedIndex > 0)
							comboBox.SelectedIndex--;
						break;

					case Key.Down:
						// If the index isn't memorized yet...
						if (item.LastSelectedIndex == null)
							// Do it now
							item.LastSelectedIndex = comboBox.SelectedIndex;

						// If there are items below, go down
						if (comboBox.SelectedIndex < comboBox.Items.Count - 1)
							comboBox.SelectedIndex++;
						break;
				}
			}

			// Otherwise...
			else
			{
				// If the down key has been pressed...
				if (e.Key == Key.Down)
				{
					// Open it
					comboBox.IsDropDownOpen = true;

					// If there are some items and none of them are selected...
					if (comboBox.Items.Count > 0 && comboBox.SelectedIndex == -1)
						// Select the first one
						comboBox.SelectedIndex = 0;
				}
			}
		}
	}

	/// <summary>
	/// Maps values from <see cref="ComboBox.Text"/> into editable <see cref="TextBox"/>'s text.
	/// </summary>
	public class SelectionMapperProperty : BaseAttachedProperty<SelectionMapperProperty, bool>
	{
		private readonly ToggleButtonAttachedPropertiesHelper<bool> helper;

		/// <summary>
		/// Creates an instance of the attached property.
		/// </summary>
		public SelectionMapperProperty() => helper = new ToggleButtonAttachedPropertiesHelper<bool>(true, OnSetup);

		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
			=> helper.OnValueChanged(sender, e);

		private void OnSetup(StoredItem<bool> item, object selectedItem)
		{
			// Remove any previous event handlers
			item.ComboBox.PreviewMouseLeftButtonDown -= ComboBox_PreviewMouseLeftButtonDown;

			// If the textbox should be selection mapped...
			if (item.Value)
			{
				// If there is an initial value...
				if (selectedItem != null)
					// Set it
					item.TextBox.SetTextAndMoveCaretToEnd(item.ComboBox.GetItemText(selectedItem));

				// Start listening for combobox LMB clicks
				item.ComboBox.PreviewMouseLeftButtonDown += ComboBox_PreviewMouseLeftButtonDown;
			}
		}

		private void ComboBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var comboBox = helper[sender];

			if (comboBox.IsDropDownOpen)
			{
				var propName = nameof(FrameworkElement.DataContext);

				void Close(object item)
				{
					var dataContext = item?.GetType().GetProperty(propName)?.GetValue(item);

					if
					(
						dataContext != null &&
						(
							comboBox.ItemsSource == null ||
							comboBox.ItemsSource.GetType().GetGenericArguments()[0] == dataContext.GetType()
						)
					)
						helper.Find(comboBox).CloseComboBoxAndShowText(comboBox.GetItemText(dataContext));
				}

				Close(e.OriginalSource);
				Close(e.Source);
			}
		}
	}

	/// <summary>
	/// Used for adding items to the <see cref="ComboBox"/> when the <see cref="TextBox"/> loses focus.
	/// </summary>
	public class LostFocusFillerProperty : BaseAttachedProperty<LostFocusFillerProperty, string>
	{
		private static ToggleButtonAttachedPropertiesHelper<string> _helper;

		/// <summary>
		/// Creates an instance of the attached property.
		/// </summary>
		public LostFocusFillerProperty() => _helper = new ToggleButtonAttachedPropertiesHelper<string>(true, OnSetup);

		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
			=> _helper.OnValueChanged(sender, e);

		private void OnSetup(StoredItem<string> item)
		{
			// Remove any previous event handlers
			item.TextBox.LostFocus -= TextBox_LostFocus;

			// If the textbox should be a filler...
			if (item.Value.IsNotNullOrWhiteSpace())
			{
				// Start listening for focus changes
				item.TextBox.LostFocus += TextBox_LostFocus;
			}
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			// Get the saved combobox
			var comboBox = _helper[sender];

			// Construct the item from the combobox
			Construct(comboBox);
		}

		/// <summary>
		/// Uses item construction method on the data context of the
		/// <see cref="ComboBox"/> to perform changes.
		/// <para>The construction method must have return type of <see cref="bool"/> and
		/// take in two parameters - first of type string (text from <see cref="TextBox"/>) and
		/// second of type <see cref="object"/> (items source of the <see cref="ComboBox"/>).</para>
		/// </summary>
		/// <param name="comboBox"><see cref="ComboBox"/> for which to call the method.</param>
		/// <returns>If the construction went through successfully.</returns>
		public static bool Construct(ComboBox comboBox)
		{
			// Get the stored item
			var item = _helper.Find(comboBox);

			// Get the method name
			var method = item.Value;

			// If method isn't provided...
			if (method.IsNullOrWhiteSpace())
				// Signal error
				return false;

			// Get the method that will create the item
			var constructItemMethod = comboBox.DataContext?
				.GetType()
				.GetAMethod
				((m, parameters) =>
					m.Name == method &&
					m.ReturnType == typeof(bool) &&
					parameters.Length == 2 &&
					parameters[0].ParameterType == typeof(string) &&
					parameters[1].ParameterType == typeof(object)
				);

			// If it exists...
			if (constructItemMethod != null)
			{
				// Invoke it with text inside the textbox and items from the combo box
				var result = (bool)constructItemMethod.Invoke(comboBox.DataContext, new object[] { item.TextBox.Text, comboBox.ItemsSource });

				// Return the result of the method
				return result;
			}

			// Method wasn't found, bail
			return false;
		}
	}

	/// <summary>
	/// Used for enabling automcomplete while typing in the textbox. It can be
	/// implemented on the data context of combobox (method must be named <see cref="customAutoCompleteMethodName"/>,
	/// take in current text from textbox and items in the combobox and return index of the item that should be selected
	/// or -1 if none should be).
	/// </summary>
	public class AutoCompleteProperty : BaseAttachedProperty<AutoCompleteProperty, bool>
	{
		private const string customAutoCompleteMethodName = "AutoComplete";

		private readonly ToggleButtonAttachedPropertiesHelper<bool> helper;

		/// <summary>
		/// Creates an instance of the attached property.
		/// </summary>
		public AutoCompleteProperty() => helper = new ToggleButtonAttachedPropertiesHelper<bool>(true, OnSetup);

		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
			=> helper.OnValueChanged(sender, e);

		private void OnSetup(StoredItem<bool> item)
		{
			// Remove any previous event handlers
			item.TextBox.TextChanged -= TextBox_TextChanged;

			// If the autocomplete should be turned on...
			if (item.Value)
			{
				// Start listening for text changes
				item.TextBox.TextChanged += TextBox_TextChanged;
			}
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Get the saved combobox
			var comboBox = helper[sender];

			var autoCompleteMethod = comboBox.DataContext?
				.GetType()
				.GetMethods()
				.FirstOrDefault(m =>
				{
					var parameters = m.GetParameters();

					return
						m.Name == customAutoCompleteMethodName &&
						m.ReturnType == typeof(int) &&
						parameters.Length == 2 &&
						parameters[0].ParameterType == typeof(string) &&
						parameters[1].ParameterType == typeof(object);
				});

			var index = -1;
			var input = (sender as TextBox).Text;

			if (autoCompleteMethod != null)
				index = (int)autoCompleteMethod.Invoke(comboBox.DataContext, new object[] { input, comboBox.ItemsSource });
			else
			{
				var i = 0;

				foreach (var item in comboBox.Items)
				{
					var display = comboBox.GetItemText(item);

					if (display.StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
					{
						index = i;
						break;
					}

					i++;
				}
			}

			if (index >= 0 && index < comboBox.Items.Count)
			{
				comboBox.SelectedIndex = index;
				comboBox.IsDropDownOpen = true;
			}
		}
	}
}
