using Braco.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for xaml
	/// </summary>
	public partial class ImageButton : UserControl
	{
		/// <summary>
		/// Name of the <see cref="WpfImage"/> control.
		/// </summary>
		public const string ImageControlName = "TheImage";

		#region Image Button Specific Dependency Properties

		/// <summary>
		/// Determines if the button is pressed or not.
		/// </summary>
		public bool IsPressed
		{
			get { return (bool)GetValue(IsPressedProperty); }
			set { SetValue(IsPressedProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="IsPressed"/>.
		/// </summary>
		public static readonly DependencyProperty IsPressedProperty =
			DependencyProperty.Register(nameof(IsPressed), typeof(bool), typeof(ImageButton), new PropertyMetadata(false));

		/// <summary>
		/// Size of the button that contains the image.
		/// <para>Make sure to use a value equal to or greater than 0 in order to change the size.</para>
		/// </summary>
		public string ButtonSize
		{
			get { return (string)GetValue(ButtonSizeProperty); }
			set { SetValue(ButtonSizeProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ButtonSize"/>.
		/// </summary>
		public static readonly DependencyProperty ButtonSizeProperty =
			DependencyProperty.Register(nameof(ButtonSize), typeof(string), typeof(ImageButton), new PropertyMetadata("20"));

		/// <summary>
		/// <see cref="ImageSource"/> to use for the image.
		/// </summary>
		public ImageSource Source
		{
			get { return (ImageSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Source"/>.
		/// </summary>
		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(new PropertyChangedCallback(OnSourceChanged)));

		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not ImageButton imageButton) return;
			
			var newSource = e.NewValue as ImageSource;

			var exists = newSource != null;

			imageButton.Visibility = VisibilityHelpers.Convert(exists, null);

			if (exists)
			{
				var image = ControlTree.FindChild<Image>(imageButton);

				if (image != null)
				{
					image.Source = newSource;
				}
			}
		}

		/// <summary>
		/// Command to execute on click.
		/// </summary>
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Command"/>.
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ImageButton), new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is not ICommand command || d is not ImageButton btn || btn.Button.Command == command) return;

			btn.Button.Command = new RelayCommand(() =>
			{
				command.Execute(btn.Button.CommandParameter);

				if (btn.UpdateImageSourceAfterExecutingCommand)
				{
					var image = ControlTree.FindChild<Image>(btn);

					if (image != null)
					{
						image.UpdateSource();
					}
				}
			});
		}

		/// <summary>
		/// Parameter for the command.
		/// </summary>
		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="CommandParameter"/>.
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty =
			DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ImageButton), new PropertyMetadata(null));

		/// <summary>
		/// Defines if the image source should be updated upon executing the <see cref="Command"/>.
		/// </summary>
		public bool UpdateImageSourceAfterExecutingCommand
		{
			get { return (bool)GetValue(UpdateImageSourceAfterExecutingCommandProperty); }
			set { SetValue(UpdateImageSourceAfterExecutingCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="UpdateImageSourceAfterExecutingCommand"/>.
		/// </summary>
		public static readonly DependencyProperty UpdateImageSourceAfterExecutingCommandProperty =
			DependencyProperty.Register(nameof(UpdateImageSourceAfterExecutingCommand), typeof(bool), typeof(ImageButton), new PropertyMetadata(false));

		#endregion

		#region Image Dependency Properties

		/// <summary>
		/// Determines if the image is an svg or not.
		/// </summary>
		public bool IsSvg
		{
			get { return (bool)GetValue(IsSvgProperty); }
			set { SetValue(IsSvgProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="IsSvg"/>.
		/// </summary>
		public static readonly DependencyProperty IsSvgProperty =
			DependencyProperty.Register(nameof(IsSvg), typeof(bool), typeof(ImageButton), new PropertyMetadata(true));

		/// <summary>
		/// Path to the image file.
		/// </summary>
		public string Path
		{
			get { return (string)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Path"/>.
		/// </summary>
		public static readonly DependencyProperty PathProperty =
			DependencyProperty.Register(nameof(Path), typeof(string), typeof(ImageButton), new PropertyMetadata(null));

		/// <summary>
		/// Subfolder in which the image resides.
		/// </summary>
		public string Subfolder
		{
			get => (string)GetValue(SubfolderProperty);
			set => SetValue(SubfolderProperty, value);
		}

		/// <summary>
		/// Dependency property for <see cref="Subfolder"/>.
		/// </summary>
		public static readonly DependencyProperty SubfolderProperty =
			DependencyProperty.Register(nameof(Subfolder), typeof(string), typeof(ImageButton), new PropertyMetadata(null));

		/// <summary>
		/// File name for the image.
		/// </summary>
		public string FileName
		{
			get => (string)GetValue(FileNameProperty);
			set => SetValue(FileNameProperty, value);
		}

		/// <summary>
		/// Dependency property for <see cref="FileName"/>.
		/// </summary>
		public static readonly DependencyProperty FileNameProperty =
			DependencyProperty.Register(nameof(FileName), typeof(string), typeof(ImageButton), new PropertyMetadata(new PropertyChangedCallback(OnFileNameChanged)));

		private static void OnFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ImageButton imageButton && e.NewValue is string fileName)
			{
				var toolTipKey = DI.Resources?.Get<ToolTipGetter>(fileName);

				if (toolTipKey != null)
				{
					imageButton.SetResourceReference(ToolTipProperty, toolTipKey);
				}
			}
		}

		/// <summary>
		/// Extension of the image file.
		/// </summary>
		public string Extension
		{
			get => (string)GetValue(ExtensionProperty);
			set => SetValue(ExtensionProperty, value);
		}

		/// <summary>
		/// Dependency property for <see cref="Extension"/>.
		/// </summary>
		public static readonly DependencyProperty ExtensionProperty =
			DependencyProperty.Register(nameof(Extension), typeof(string), typeof(ImageButton), new PropertyMetadata(Image.PngExtension));

		/// <summary>
		/// Getter used to determine the file name.
		/// </summary>
		public string FileNameGetter
		{
			get { return (string)GetValue(FileNameGetterProperty); }
			set { SetValue(FileNameGetterProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="FileNameGetter"/>.
		/// </summary>
		public static readonly DependencyProperty FileNameGetterProperty =
			DependencyProperty.Register(nameof(FileNameGetter), typeof(string), typeof(ImageButton), new PropertyMetadata(null));

		/// <summary>
		/// Parameter to use for <see cref="FileNameGetter"/>
		/// </summary>
		public object FileNameGetterParameter
		{
			get { return GetValue(FileNameGetterParameterProperty); }
			set { SetValue(FileNameGetterParameterProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="FileNameGetterParameter"/>.
		/// </summary>
		public static readonly DependencyProperty FileNameGetterParameterProperty =
			DependencyProperty.Register(nameof(FileNameGetterParameter), typeof(object), typeof(ImageButton), new PropertyMetadata(null));

		/// <summary>
		/// If set to true, <see cref="System.Windows.Media.Imaging.BitmapImage"/> will load the image and it will immediately
		/// close the file after the image has fully loaded, thus releasing the handle for the file after loading.
		/// <para>Note: this doesn't apply if <see cref="IsSvg"/> is true because SVGs are not loaded using <see cref="System.Windows.Media.Imaging.BitmapImage"/>.</para>
		/// </summary>
		public bool CloseFileAfterLoad
		{
			get { return (bool)GetValue(CloseFileAfterLoadProperty); }
			set { SetValue(CloseFileAfterLoadProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="CloseFileAfterLoad"/>.
		/// </summary>
		public static readonly DependencyProperty CloseFileAfterLoadProperty =
			DependencyProperty.Register(nameof(CloseFileAfterLoad), typeof(bool), typeof(ImageButton), new PropertyMetadata(false));

		/// <summary>
		/// Defines visibility parameter for <see cref="VisibilityHelpers.Convert(bool, object)"/>.
		/// </summary>
		public object VisibilityParameter
		{
			get { return GetValue(VisibilityParameterProperty); }
			set { SetValue(VisibilityParameterProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="VisibilityParameter"/>.
		/// </summary>
		public static readonly DependencyProperty VisibilityParameterProperty =
			DependencyProperty.Register(nameof(VisibilityParameter), typeof(object), typeof(ImageButton), new PropertyMetadata(null));

		#endregion

		/// <summary>
		/// Creates an instance of the control.
		/// </summary>
		public ImageButton()
		{
			InitializeComponent();

			Button.PreviewMouseDown += (sender, e) => IsPressed = true;
			Button.PreviewMouseUp += (sender, e) => IsPressed = false;
		}
	}
}
