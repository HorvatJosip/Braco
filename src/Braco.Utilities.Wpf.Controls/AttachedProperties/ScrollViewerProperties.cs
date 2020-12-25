using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Used for allowing <see cref="ScrollViewer"/> to always scroll within its area, even if the mouse
	/// is over some element that might be blocking scrolling.
	/// </summary>
	public class TopScrollPriorityProperty : BaseAttachedProperty<TopScrollPriorityProperty, bool>
	{
		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is not ScrollViewer scrollViewer) return;

			scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;

			if ((bool)e.NewValue)
			{
				scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
			}
		}

		private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
		{
			var scrollViewer = (ScrollViewer)sender;
			scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
			e.Handled = true;
		}
	}
}
