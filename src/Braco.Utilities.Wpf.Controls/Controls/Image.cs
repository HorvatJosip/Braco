using System.Windows;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Extends the <see cref="System.Windows.Controls.Image"/> with few properties
	/// which are used for setting its source.
	/// </summary>
	public class Image : System.Windows.Controls.Image
	{
		/// <summary>
		/// Png file extension.
		/// </summary>
		public const string PngExtension = "png";

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
			DependencyProperty.Register(nameof(IsSvg), typeof(bool), typeof(Image), new PropertyMetadata(true, new PropertyChangedCallback(UpdateSource)));

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
			DependencyProperty.Register(nameof(Path), typeof(string), typeof(Image), new PropertyMetadata(new PropertyChangedCallback(UpdateSource)));

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
			DependencyProperty.Register(nameof(Subfolder), typeof(string), typeof(Image), new PropertyMetadata(new PropertyChangedCallback(UpdateSource)));

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
			DependencyProperty.Register(nameof(FileName), typeof(string), typeof(Image), new PropertyMetadata(new PropertyChangedCallback(UpdateSource)));

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
			DependencyProperty.Register(nameof(Extension), typeof(string), typeof(Image), new PropertyMetadata(PngExtension, new PropertyChangedCallback(UpdateSource)));

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
			DependencyProperty.Register(nameof(FileNameGetter), typeof(string), typeof(Image), new PropertyMetadata(new PropertyChangedCallback(UpdateSource)));

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
			DependencyProperty.Register(nameof(FileNameGetterParameter), typeof(object), typeof(Image), new PropertyMetadata(new PropertyChangedCallback(UpdateSource)));

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
			DependencyProperty.Register(nameof(CloseFileAfterLoad), typeof(bool), typeof(Image), new PropertyMetadata(new PropertyChangedCallback(UpdateSource)));

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
			DependencyProperty.Register(nameof(VisibilityParameter), typeof(object), typeof(Image), new PropertyMetadata(null));

		private static void UpdateSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is Image image)
			{
				image.UpdateSource();
			}
		}

		/// <summary>
		/// Updates <see cref="Source"/> using the following dependency properties:
		/// <para><see cref="Subfolder"/>, <see cref="FileName"/>, <see cref="FileNameGetter"/>,
		/// <see cref="Extension"/>, <see cref="IsSvg"/>, <see cref="CloseFileAfterLoad"/> and
		/// <see cref="FileNameGetterParameter"/>. The <see cref="Visibility"/> of the control will be
		/// changed based on if it is defined and <see cref="VisibilityParameter"/> (see <see cref="VisibilityHelpers.Convert(bool, object)"/>).</para>
		/// </summary>
		public void UpdateSource()
		{
			Source = ImageSourceGetter.Get(Path, Subfolder, FileName, FileNameGetter, Extension, IsSvg, CloseFileAfterLoad, () => new object[]
			{
				DataContext,
				ControlTree.FindAncestor<System.Windows.Controls.Page>(this)?.DataContext,
				FileNameGetterParameter
			});

			Visibility = VisibilityHelpers.Convert(Source != null, VisibilityParameter);
		}
	}
}
