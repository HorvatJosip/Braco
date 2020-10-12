using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Represents an input field with a label.
	/// </summary>
	public class InputField : ContentControl
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
			DependencyProperty.Register(nameof(Label), typeof(string), typeof(InputField), new PropertyMetadata(null));

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
			DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(InputField), new PropertyMetadata(false));

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
			DependencyProperty.Register(nameof(Info), typeof(string), typeof(InputField), new PropertyMetadata(null));

		/// <summary>
		/// Content placed to the right of the label.
		/// </summary>
		public object ContentToTheRight
		{
			get { return (object)GetValue(ContentToTheRightProperty); }
			set { SetValue(ContentToTheRightProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ContentToTheRight"/>.
		/// </summary>
		public static readonly DependencyProperty ContentToTheRightProperty =
			DependencyProperty.Register(nameof(ContentToTheRight), typeof(object), typeof(InputField), new PropertyMetadata(null));

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
			DependencyProperty.Register(nameof(PanelMargin), typeof(Thickness), typeof(InputField), new PropertyMetadata(new Thickness(0, 20, 0, 0)));

	}
}
