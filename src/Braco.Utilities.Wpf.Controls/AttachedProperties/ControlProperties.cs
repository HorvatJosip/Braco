using Braco.Utilities.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Used for setting focus on a control.
	/// </summary>
	public class SetFocusProperty : BaseAttachedProperty<SetFocusProperty, bool>
	{
		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			// If the focus prerequisites aren't present...
			if (!(e.NewValue is bool shouldFocus && sender is Control control))
				// Bail
				return;

			// Remove previous subscriptions
			control.Loaded -= Control_Loaded;

			// If the focus should be added to the control...
			if (shouldFocus)
			{
				// Subscribe to the loaded event
				control.Loaded += Control_Loaded;
			}
		}

		private void Control_Loaded(object sender, RoutedEventArgs e)
		{
			// Get the control from the sender
			var control = (Control)sender;

			// Make it focus
			control.Focusable = true;
			control.Focus();
		}
	}

	/// <summary>
	/// Used for setting up the drop command on the <see cref="Page"/> data context to
	/// execute once something is dropped onto the control.
	/// </summary>
	public class DropCommandProperty : BaseAttachedProperty<DropCommandProperty, string>
	{
		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(e.NewValue is string commandName && sender is UIElement uiElement))
				return;

			void Control_Drop(object sender, DragEventArgs e)
			{
				var viewModel = ControlTree.FindAncestor<Page>((DependencyObject)sender)?.DataContext;

				if 
				(
					viewModel != null &&
					e.Data.GetDataPresent(DataFormats.FileDrop) &&
					viewModel.GetType().GetProperty(commandName)?.GetValue(viewModel) is ICommand command
				)
					command.Execute(e.Data.GetData(DataFormats.FileDrop));
			}

			uiElement.Drop -= Control_Drop;

			if (commandName.IsNotNullOrWhiteSpace())
			{
				uiElement.AllowDrop = true;
				uiElement.Drop += Control_Drop;
			}
		}
	}
}
