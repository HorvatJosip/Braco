using Braco.Utilities.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Helper for working with attached properties of a toggle button
	/// (part of combo box).
	/// </summary>
	/// <typeparam name="T">Type of value for <see cref="StoredItem{T}"/>.</typeparam>
	public class ToggleButtonAttachedPropertiesHelper<T>
	{
		private readonly bool _editable;
		private readonly Action<StoredItem<T>, object> _onReady;

		/// <summary>
		/// Collection of items 
		/// </summary>
		public IList<StoredItem<T>> Storage { get; } = new List<StoredItem<T>>();

		/// <summary>
		/// Gets the <see cref="ComboBox"/> for the given item.
		/// </summary>
		/// <param name="item">A property from <see cref="StoredItem{T}"/> that
		/// contains the <see cref="ComboBox"/>.</param>
		/// <returns><see cref="ComboBox"/> from <see cref="StoredItem{T}"/>
		/// if it is found.</returns>
		public ComboBox this[object item] => Find(item)?.ComboBox;

		/// <summary>
		/// Creates the helper.
		/// </summary>
		/// <param name="editable">Is the <see cref="ComboBox"/> editable?</param>
		/// <param name="onReady">Action to perform once data is loaded.</param>
		public ToggleButtonAttachedPropertiesHelper(bool editable, Action<StoredItem<T>> onReady)
			: this(editable, (item, _) => onReady(item)) { }

		/// <summary>
		/// Creates the helper.
		/// </summary>
		/// <param name="editable">Is the <see cref="ComboBox"/> editable?</param>
		/// <param name="onReady">Action to perform once data is loaded. The second parameter
		/// is selected item from the <see cref="ComboBox"/>.</param>
		public ToggleButtonAttachedPropertiesHelper(bool editable, Action<StoredItem<T>, object> onReady)
		{
			_editable = editable;
			_onReady = onReady ?? throw new ArgumentNullException(nameof(onReady));
		}

		/// <summary>
		/// Handles value changed event of the attached property.
		/// </summary>
		/// <param name="sender">Object that raised the event.</param>
		/// <param name="e">Arguments for the event.</param>
		public void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			// If sender isn't the toggle button...
			if (sender is not ToggleButton toggleButton)
				// Bail
				return;

			Storage.Add(new StoredItem<T>(toggleButton));

			// Wait for the toggle button to load...
			toggleButton.Loaded += (obj, args) =>
			{
				// Get the item
				var item = Find(obj);

				// Fetch the down arrow
				var downArrow = ControlTree.FindChild<ImageButton>(item.ToggleButton);

				// Extract the new value
				var value = (T)e.NewValue;

				// Find the combo box relative to the toggle button
				var comboBox = ControlTree.FindAncestor<ComboBox>(item.ToggleButton);

				// If the combobox or down arrow isn't found...
				if (comboBox == null || downArrow == null)
					// Bail
					return;

				if (_editable == false)
				{
					// Setup the rest of the data
					item.SetupRest(downArrow, comboBox, value);

					// Call the method with everything setup
					_onReady(item, comboBox.SelectedItem);
				}
				else
				{
					// Find the child textbox
					var textBox = ControlTree.FindChild<TextBox>(comboBox);

					// If it isn't found...
					if (textBox == null)
						// Bail
						return;

					// Setup the rest of the data
					item.SetupRest(downArrow, comboBox, textBox, value);

					// Call the method with everything setup
					_onReady(item, comboBox.SelectedItem);
				}
			};
		}

		/// <summary>
		/// Finds the <see cref="StoredItem{T}"/> based on one
		/// of its properties.
		/// </summary>
		/// <param name="item">Item that needs to match one of
		/// the properties of a <see cref="StoredItem{T}"/>.</param>
		/// <returns><see cref="StoredItem{T}"/> if given item is part of it.</returns>
		public StoredItem<T> Find(object item) => Storage.FirstOrDefault(storedItem =>
		{
			foreach (var prop in storedItem.GetType().GetProperties())
			{
				var value = prop.GetValue(storedItem);

				if (Equals(value, item))
					return true;
			}

			return false;
		});
	}
}
