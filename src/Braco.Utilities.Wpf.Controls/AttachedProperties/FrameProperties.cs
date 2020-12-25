using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Used for disabling navigation.
	/// </summary>
	public class NoNavigationProperty : BaseAttachedProperty<NoNavigationProperty, bool>
	{
		/// <inheritdoc/>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is not bool noNavigation || sender is not Frame frame) return;

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
