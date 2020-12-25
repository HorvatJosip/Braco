using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// HasText attached property for <see cref="PasswordBox"/>.
	/// True if the <see cref="PasswordBox"/> that it is attached to
	/// has some text in it.
	/// </summary>
	public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>
	{
		/// <summary>
		/// Sets the HasText property based on if the caller
		/// <see cref="PasswordBox"/> has any text.
		/// </summary>
		/// <param name="sender">Pa</param>
		public static void SetValue(DependencyObject sender)
		{
			// If the incoming ui element isn't a passwordbox...
			if (sender is not PasswordBox passwordBox)
				// Bail
				return;

			// Set the value based on the length of the password
			SetValue(passwordBox, passwordBox.SecurePassword.Length > 0);
		}
	}

	/// <summary>
	/// MonitorPassword attached property for <see cref="PasswordBox"/>.
	/// </summary>
	public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
	{
		/// <summary>
		/// Fired whenever the MonitorPassword property changes.
		/// </summary>
		/// <param name="sender">Object for which the MonitorPassword property value changed.</param>
		/// <param name="e">Arguments for the event.</param>
		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			// If the incoming ui element isn't a passwordbox...
			if (sender is not PasswordBox passwordBox)
				// Bail
				return;

			// Remove any previous event handlers
			passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

			// If the password should be monitored...
			if ((bool)e.NewValue)
			{
				HasTextProperty.SetValue(passwordBox);

				// Start listening for password changes
				passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
			}
		}

		/// <summary>
		/// Fired when the <see cref="PasswordBox"/>'s password changes.
		/// </summary>
		/// <param name="sender"><see cref="PasswordBox"/> that had its password changed.</param>
		/// <param name="e">Arguments for the event.</param>
		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			HasTextProperty.SetValue(sender as DependencyObject);
		}
	}
}
