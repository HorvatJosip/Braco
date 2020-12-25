using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for TitleBar.xaml
	/// </summary>
	public partial class TitleBar : UserControl
	{
		/// <summary>
		/// Command to fire once the settings button was clicked.
		/// </summary>
		public ICommand SettingsCommand
		{
			get { return (ICommand)GetValue(SettingsCommandProperty); }
			set { SetValue(SettingsCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="SettingsCommand"/>.
		/// </summary>
		public static readonly DependencyProperty SettingsCommandProperty =
			DependencyProperty.Register(nameof(SettingsCommand), typeof(ICommand), typeof(TitleBar), new PropertyMetadata(null));

		/// <summary>
		/// Determines if the settings button will be visible.
		/// </summary>
		public Visibility SettingsVisibility
        {
            get { return (Visibility)GetValue(SettingsVisibilityProperty); }
            set { SetValue(SettingsVisibilityProperty, value); }
        }

		/// <summary>
		/// Dependency property for <see cref="SettingsVisibility"/>.
		/// </summary>
		public static readonly DependencyProperty SettingsVisibilityProperty =
            DependencyProperty.Register(nameof(SettingsVisibility), typeof(Visibility), typeof(TitleBar), new PropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Command to fire once the minimize button was clicked.
		/// </summary>
		public ICommand MinimizeCommand
		{
			get { return (ICommand)GetValue(MinimizeCommandProperty); }
			set { SetValue(MinimizeCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="MinimizeCommand"/>.
		/// </summary>
		public static readonly DependencyProperty MinimizeCommandProperty =
			DependencyProperty.Register(nameof(MinimizeCommand), typeof(ICommand), typeof(TitleBar), new PropertyMetadata(null));

		/// <summary>
		/// Determines if the minimize button will be visible.
		/// </summary>
		public Visibility MinimizeVisibility
        {
            get { return (Visibility)GetValue(MinimizeVisibilityProperty); }
            set { SetValue(MinimizeVisibilityProperty, value); }
        }

		/// <summary>
		/// Dependency property for <see cref="MinimizeVisibility"/>.
		/// </summary>
		public static readonly DependencyProperty MinimizeVisibilityProperty =
            DependencyProperty.Register(nameof(MinimizeVisibility), typeof(Visibility), typeof(TitleBar), new PropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Command to fire once the maximize button was clicked.
		/// </summary>
		public ICommand MaximizeCommand
		{
			get { return (ICommand)GetValue(MaximizeCommandProperty); }
			set { SetValue(MaximizeCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="MaximizeCommand"/>.
		/// </summary>
		public static readonly DependencyProperty MaximizeCommandProperty =
			DependencyProperty.Register(nameof(MaximizeCommand), typeof(ICommand), typeof(TitleBar), new PropertyMetadata(null));

		/// <summary>
		/// Determines if the maximize button will be visible.
		/// </summary>
		public Visibility MaximizeVisibility
        {
            get { return (Visibility)GetValue(MaximizeVisibilityProperty); }
            set { SetValue(MaximizeVisibilityProperty, value); }
        }

		/// <summary>
		/// Dependency property for <see cref="MaximizeVisibility"/>.
		/// </summary>
		public static readonly DependencyProperty MaximizeVisibilityProperty =
            DependencyProperty.Register(nameof(MaximizeVisibility), typeof(Visibility), typeof(TitleBar), new PropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Command to fire once the close button was clicked.
		/// </summary>
		public ICommand CloseCommand
		{
			get { return (ICommand)GetValue(CloseCommandProperty); }
			set { SetValue(CloseCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="CloseCommand"/>.
		/// </summary>
		public static readonly DependencyProperty CloseCommandProperty =
			DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(TitleBar), new PropertyMetadata(null));

		/// <summary>
		/// Determines if the close button will be visible.
		/// </summary>
		public Visibility CloseVisibility
		{
			get { return (Visibility)GetValue(CloseVisibilityProperty); }
			set { SetValue(CloseVisibilityProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="CloseVisibility"/>.
		/// </summary>
		public static readonly DependencyProperty CloseVisibilityProperty =
			DependencyProperty.Register(nameof(CloseVisibility), typeof(Visibility), typeof(TitleBar), new PropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Command to fire once the bar was clicked.
		/// </summary>
		public ICommand ClickCommand
		{
			get { return (ICommand)GetValue(ClickCommandProperty); }
			set { SetValue(ClickCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ClickCommand"/>.
		/// </summary>
		public static readonly DependencyProperty ClickCommandProperty =
			DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(TitleBar), new PropertyMetadata(null));
		
		/// <summary>
		/// Command to fire once the bar was double clicked.
		/// </summary>
		public ICommand DoubleClickCommand
		{
			get { return (ICommand)GetValue(DoubleClickCommandProperty); }
			set { SetValue(DoubleClickCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="DoubleClickCommand"/>.
		/// </summary>
		public static readonly DependencyProperty DoubleClickCommandProperty =
			DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(TitleBar), new PropertyMetadata(null));

		/// <summary>
		/// Defines the title.
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
			DependencyProperty.Register(nameof(Title), typeof(string), typeof(TitleBar), new PropertyMetadata(null));
		
		/// <summary>
		/// Defines the <see cref="ImageSource"/> of the logo.
		/// </summary>
		public ImageSource Logo
		{
			get { return (ImageSource)GetValue(LogoProperty); }
			set { SetValue(LogoProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Logo"/>.
		/// </summary>
		public static readonly DependencyProperty LogoProperty =
			DependencyProperty.Register(nameof(Logo), typeof(ImageSource), typeof(TitleBar), new PropertyMetadata(null));

		/// <summary>
		/// ToolTip for the close button.
		/// </summary>
		public string CloseToolTip
		{
			get { return (string)GetValue(CloseToolTipProperty); }
			set { SetValue(CloseToolTipProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="CloseToolTip"/>.
		/// </summary>
		public static readonly DependencyProperty CloseToolTipProperty =
			DependencyProperty.Register(nameof(CloseToolTip), typeof(string), typeof(TitleBar), new PropertyMetadata(null));

		/// <summary>
		/// Size of icons on the right.
		/// </summary>
		public string IconSize
		{
			get { return (string)GetValue(IconSizeProperty); }
			set { SetValue(IconSizeProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="IconSize"/>.
		/// </summary>
		public static readonly DependencyProperty IconSizeProperty =
			DependencyProperty.Register(nameof(IconSize), typeof(string), typeof(TitleBar), new PropertyMetadata("22"));

		/// <summary>
		/// Width of logo on the left.
		/// </summary>
		public double LogoWidth
		{
			get { return (double)GetValue(LogoWidthProperty); }
			set { SetValue(LogoWidthProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="LogoWidth"/>.
		/// </summary>
		public static readonly DependencyProperty LogoWidthProperty =
			DependencyProperty.Register(nameof(LogoWidth), typeof(double), typeof(TitleBar), new PropertyMetadata(32.0));

		/// <summary>
		/// Height of logo on the left.
		/// </summary>
		public double LogoHeight
		{
			get { return (double)GetValue(LogoHeightProperty); }
			set { SetValue(LogoHeightProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="LogoHeight"/>.
		/// </summary>
		public static readonly DependencyProperty LogoHeightProperty =
			DependencyProperty.Register(nameof(LogoHeight), typeof(double), typeof(TitleBar), new PropertyMetadata(32.0));
		
		/// <summary>
		/// Creates an instance of the control.
		/// </summary>
		public TitleBar()
        {
            InitializeComponent();
        }
    }
}
