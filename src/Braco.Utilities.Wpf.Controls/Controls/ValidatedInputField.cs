using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Represents an input field with a label within a validated field.
	/// </summary>
	public class ValidatedInputField : ContentControl
	{
		/// <summary>
		/// Label for the input field.
		/// </summary>
		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Label"/>.
		/// </summary>
		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register(nameof(Label), typeof(string), typeof(ValidatedInputField), new PropertyMetadata(null));

		/// <summary>
		/// Determines if the field is required or not.
		/// </summary>
		public bool IsRequired
		{
			get { return (bool)GetValue(IsRequiredProperty); }
			set { SetValue(IsRequiredProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="IsRequired"/>.
		/// </summary>
		public static readonly DependencyProperty IsRequiredProperty =
			DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(ValidatedInputField), new PropertyMetadata(false));

		/// <summary>
		/// Info about the field.
		/// </summary>
		public string Info
		{
			get { return (string)GetValue(InfoProperty); }
			set { SetValue(InfoProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Info"/>.
		/// </summary>
		public static readonly DependencyProperty InfoProperty =
			DependencyProperty.Register(nameof(Info), typeof(string), typeof(ValidatedInputField), new PropertyMetadata(null));

		/// <summary>
		/// Content placed to the right of the label.
		/// </summary>
		public object ContentToTheRight
		{
			get { return GetValue(ContentToTheRightProperty); }
			set { SetValue(ContentToTheRightProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ContentToTheRight"/>.
		/// </summary>
		public static readonly DependencyProperty ContentToTheRightProperty =
			DependencyProperty.Register(nameof(ContentToTheRight), typeof(object), typeof(ValidatedInputField), new PropertyMetadata(null));

		/// <summary>
		/// Margin used for the top panel.
		/// </summary>
		public Thickness PanelMargin
		{
			get { return (Thickness)GetValue(PanelMarginProperty); }
			set { SetValue(PanelMarginProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="PanelMargin"/>.
		/// </summary>
		public static readonly DependencyProperty PanelMarginProperty =
			DependencyProperty.Register(nameof(PanelMargin), typeof(Thickness), typeof(ValidatedInputField), new PropertyMetadata(new Thickness(0, 20, 0, 0)));
		
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
			DependencyProperty.Register(nameof(Validate), typeof(string), typeof(ValidatedInputField), new PropertyMetadata(null));

		/// <summary>
		/// Creates a new instance of the <see cref="ValidatedInputField"/>.
		/// </summary>
		public ValidatedInputField()
		{
			Focusable = false;
		}
	}
}
