using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Represents a field which will have validator below it.
	/// </summary>
	public class ValidatedField : ContentControl
	{
		/// <summary>
		/// Defines which property this field validates.
		/// </summary>
		public string Validate
		{
			get { return (string)GetValue(ValidateProperty); }
			set { SetValue(ValidateProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Validate"/>.
		/// </summary>
		public static readonly DependencyProperty ValidateProperty =
			DependencyProperty.Register(nameof(Validate), typeof(string), typeof(ValidatedField), new PropertyMetadata(null));

		/// <summary>
		/// Creates a new instance of the <see cref="ValidatedField"/>.
		/// </summary>
		public ValidatedField()
		{
			Focusable = false;
		}
	}
}
