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
			if (e.NewValue is not bool shouldFocus || sender is not Control control)
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
			if (e.NewValue is not string commandName || sender is not UIElement uiElement)
				return;

			void Control_Drop(object sender, DragEventArgs e)
			{
				if (sender is not FrameworkElement frameworkElement) return;

				var viewModel = ControlTree.FindAncestor<Page>(frameworkElement)?.DataContext;
				
				if 
				(
					viewModel != null &&
					e.Data.GetDataPresent(DataFormats.FileDrop) &&
					viewModel.GetType().GetProperty(commandName)?.GetValue(viewModel) is ICommand command
				)
					command.Execute(new FileDropCommandRequest(frameworkElement.DataContext, (string[])e.Data.GetData(DataFormats.FileDrop)));
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
