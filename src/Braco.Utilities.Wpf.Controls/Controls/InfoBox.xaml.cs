using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for InfoBox.xaml
	/// </summary>
	public partial class InfoBox : UserControl
	{
		/// <summary>
		/// Title of the info box.
		/// </summary>
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Title"/>.
		/// </summary>
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register(nameof(Title), typeof(string), typeof(InfoBox), new PropertyMetadata(null));

		/// <summary>
		/// Message that is placed inside the info box.
		/// </summary>
		public string Message
		{
			get { return (string)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Message"/>.
		/// </summary>
		public static readonly DependencyProperty MessageProperty =
			DependencyProperty.Register(nameof(Message), typeof(string), typeof(InfoBox), new PropertyMetadata(null));

		/// <summary>
		/// What to execute once the info box gets dismissed.
		/// </summary>
		public ICommand DismissCommand
		{
			get { return (ICommand)GetValue(DismissCommandProperty); }
			set { SetValue(DismissCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="DismissCommand"/>.
		/// </summary>
		public static readonly DependencyProperty DismissCommandProperty =
			DependencyProperty.Register(nameof(DismissCommand), typeof(ICommand), typeof(InfoBox), new PropertyMetadata(null));

		/// <summary>
		/// Should the info box be dismissed.
		/// </summary>
		public bool Dismiss
		{
			get { return (bool)GetValue(DismissProperty); }
			set { SetValue(DismissProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Dismiss"/>.
		/// </summary>
		public static readonly DependencyProperty DismissProperty =
			DependencyProperty.Register(nameof(Dismiss), typeof(bool), typeof(InfoBox), new PropertyMetadata(true));

		/// <summary>
		/// Thickness of the outer border.
		/// </summary>
		public Thickness OuterBorderThickness
		{
			get { return (Thickness)GetValue(OuterBorderThicknessProperty); }
			set { SetValue(OuterBorderThicknessProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="OuterBorderThickness"/>.
		/// </summary>
		public static readonly DependencyProperty OuterBorderThicknessProperty =
			DependencyProperty.Register(nameof(OuterBorderThickness), typeof(Thickness), typeof(InfoBox), new PropertyMetadata(default(Thickness)));

		/// <summary>
		/// Brush used on the outer border.
		/// </summary>
		public SolidColorBrush OuterBorderBrush
		{
			get { return (SolidColorBrush)GetValue(OuterBorderBrushProperty); }
			set { SetValue(OuterBorderBrushProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="OuterBorderBrush"/>.
		/// </summary>
		public static readonly DependencyProperty OuterBorderBrushProperty =
			DependencyProperty.Register(nameof(OuterBorderBrush), typeof(SolidColorBrush), typeof(InfoBox), new PropertyMetadata(null));

		/// <summary>
		/// Brush used for title and message separator.
		/// </summary>
		public SolidColorBrush SeparatorBrush
		{
			get { return (SolidColorBrush)GetValue(SeparatorBrushProperty); }
			set { SetValue(SeparatorBrushProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="SeparatorBrush"/>.
		/// </summary>
		public static readonly DependencyProperty SeparatorBrushProperty =
			DependencyProperty.Register(nameof(SeparatorBrush), typeof(SolidColorBrush), typeof(InfoBox), new PropertyMetadata(null));

		/// <summary>
		/// Creates an instance of the control.
		/// </summary>
		public InfoBox()
		{
			InitializeComponent();
		}
	}
}
