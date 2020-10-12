using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Used for disabling navigation.
	/// </summary>
	public class NoNavigation : BaseAttachedProperty<NoNavigation, bool>
	{
		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(e.NewValue is bool noNavigation && sender is Frame frame)) return;

			frame.NavigationUIVisibility = noNavigation 
				? NavigationUIVisibility.Hidden 
				: NavigationUIVisibility.Visible;

			frame.Navigated -= Frame_Navigated;

			if (noNavigation)
			{
				frame.Navigated += Frame_Navigated;
			}
		}

		private void Frame_Navigated(object sender, NavigationEventArgs e)
		{
			(sender as Frame).NavigationService.RemoveBackEntry();
		}
	}
}
