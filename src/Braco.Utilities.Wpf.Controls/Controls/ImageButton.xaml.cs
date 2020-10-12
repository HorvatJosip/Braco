using Braco.Services;
using Braco.Utilities.Extensions;
using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for xaml
	/// </summary>
	public partial class ImageButton : UserControl
	{
		/// <summary>
		/// Name of the <see cref="Image"/> control.
		/// </summary>
		public const string ImageControlName = "TheImage";

		/// <summary>
		/// Svg file extension.
		/// </summary>
		public const string SvgExtension = "svg";

		/// <summary>
		/// Png file extension.
		/// </summary>
		public const string PngExtension = "png";

		private bool _loaded;

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
		/// Determines if the button is pressed or not.
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
			DependencyProperty.Register(nameof(IsSvg), typeof(bool), typeof(ImageButton), new PropertyMetadata(true, new PropertyChangedCallback(OnIsSvgChanged)));

		private static void OnIsSvgChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ImageButton imageButton && e.NewValue is bool)
			{
				var image = ControlTree.FindChild<Image>(imageButton);

				if (image != null)
					image.Source = imageButton.GetImage();
			}
		}

		/// <summary>
		/// Subfolder in which the image resides.
		/// </summary>
		public string ImageSubfolder
		{
			get => (string)GetValue(ImageSubfolderProperty);
			set => SetValue(ImageSubfolderProperty, value);
		}

		/// <summary>
		/// Dependency property for <see cref="ImageSubfolder"/>.
		/// </summary>
		public static readonly DependencyProperty ImageSubfolderProperty =
			DependencyProperty.Register(nameof(ImageSubfolder), typeof(string), typeof(ImageButton), new PropertyMetadata(null));

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
			if (d is ImageButton imageButton)
			{
				var newSource = e.NewValue as ImageSource;

				var exists = newSource != null;

				imageButton.Visibility = VisibilityHelpers.Convert(exists, null);

				if (exists)
				{
					var image = ControlTree.FindChild<Image>(imageButton);

					if (image != null)
						image.Source = newSource;
				}
			}
		}

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
					imageButton.SetResourceReference(ToolTipProperty, toolTipKey);

				if (imageButton._loaded == false)
					return;

				var image = ControlTree.FindChild<Image>(imageButton);

				if (image != null)
					image.Source = imageButton.GetImage();
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
			DependencyProperty.Register(nameof(Extension), typeof(string), typeof(ImageButton), new PropertyMetadata(PngExtension));

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
			if (e.NewValue is ICommand command && d is ImageButton btn && btn.Button.Command != command)
				btn.Button.Command = new RelayCommand(() =>
				{
					command.Execute(btn.Button.CommandParameter);
					btn.FetchFileNameFromGetter(new object[]
					{
						ControlTree.FindAncestor<Page>(btn)?.DataContext ?? btn.DataContext,
						btn.DataContext
					});
					ControlTree.FindChild<Image>(btn.Button, ImageControlName).Source = btn.GetImage();
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
		/// Creates an instance of the control.
		/// </summary>
		public ImageButton()
		{
			InitializeComponent();

			Button.PreviewMouseDown += (sender, e) => IsPressed = true;
			Button.PreviewMouseUp += (sender, e) => IsPressed = false;
		}

		/// <summary>
		/// Method used to fetch the file name from <see cref="FileNameGetter"/>.
		/// </summary>
		/// <param name="params">Parameters for the file name getter.</param>
		public void FetchFileNameFromGetter(object[] @params)
		{
			if (FileNameGetter != null)
			{
				var parts = FileNameGetter.Split('.');

				if (parts.Length == 2)
				{
					var type = ReflectionUtilities.FindType(parts[0]);

					if (type != null)
					{
						var method = type.GetAMethod((m, parameters) =>
						(
							m.Name == parts[1] &&
							m.ReturnType == typeof(string) &&
							parameters.Length == 1 &&
							parameters[0].ParameterType == typeof(object[])
						));

						if (method?.Invoke(null, new[] { @params }) is string imageName)
							FileName = imageName;
					}
				}
			}
		}

		/// <summary>
		/// Gets the image from data.
		/// </summary>
		/// <returns>Loaded <see cref="BitmapImage"/> or null if some
		/// data isn't valid.</returns>
		public ImageSource GetImage()
		{
			if (FileName == null)
			{
				Visibility = Visibility.Collapsed;
				return null;
			}

			var name = $"{FileName}{PathUtilities.GetExtensionWithDot(IsSvg ? SvgExtension : Extension)}";
			var imagePath = DI.Resources.Get<ImageGetter, string>(ImageSubfolder, name);

			if (imagePath.IsNullOrEmpty())
			{
				Visibility = Visibility.Collapsed;
				return null;
			}

			var fullUri = new Uri(PackUtilities.GetRootPackUriWithSuffix(imagePath));

			var image = IsSvg
				? new DrawingImage(new SvgViewbox { Source = fullUri }.Drawings)
				: (ImageSource)new BitmapImage(fullUri);

			_loaded = true;
			return image;
		}
	}
}
