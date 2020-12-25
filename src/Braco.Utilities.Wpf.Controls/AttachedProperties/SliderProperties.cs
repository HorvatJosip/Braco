using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Used for smooth slider movement - once a mouse is pressed somewhere, the slider's value changes to
	/// that point and after that, if the mouse is still pressed, user can still slide to a new value.
	/// </summary>
	public class MoveToPointAndSlideProperty : BaseAttachedProperty<MoveToPointAndSlideProperty, bool>
	{
		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is not Slider slider || e.NewValue is not bool enabled) return;

			slider.IsMoveToPointEnabled = enabled;
			slider.MouseMove -= Slider_MouseMove;

			if (enabled)
			{
				slider.MouseMove += Slider_MouseMove;
			}
		}

		private void Slider_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Released) return;

			var thumb = ControlTree.FindChild<Track>(sender as Slider)?.Thumb;

			if (thumb == null || thumb.IsDragging || !thumb.IsMouseOver) return;

			thumb.RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
			{
				RoutedEvent = UIElement.MouseLeftButtonDownEvent
			});
		}
	}
}
