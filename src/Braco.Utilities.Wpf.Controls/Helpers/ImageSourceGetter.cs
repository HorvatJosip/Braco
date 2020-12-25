using Braco.Services;
using Braco.Utilities.Extensions;
using SharpVectors.Converters;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Helper class for getting image source from a file name or a file name getter.
	/// <para>File name getter is a public method on a static class that is formatted like this:</para>
	/// <para>&lt;static_class_that_contains_the_method&gt;<see cref="FileNameGetterSeparator"/>&lt;method_name&gt;</para>
	/// </summary>
	public static class ImageSourceGetter
	{
		/// <summary>
		/// Svg file extension.
		/// </summary>
		public const string SvgExtension = "svg";

		/// <summary>
		/// Separator used for defining file name getter.
		/// </summary>
		public const string FileNameGetterSeparator = ".";

		/// <summary>
		/// Gets image from a file path.
		/// </summary>
		/// <param name="path">Path to the image file.</param>
		/// <param name="isSvg">Is the image at given path an SVG?</param>
		/// <param name="closeFileAfterLoad">If set to true, <see cref="BitmapImage"/> will be used to load the image from a file
		/// and it will immediately close the file after loading.</param>
		/// <returns>Source to use for <see cref="System.Windows.Controls.Image"/>.</returns>
		public static ImageSource GetFromPath(string path, bool isSvg, bool closeFileAfterLoad)
		{
			if (path.IsNullOrWhiteSpace()) return null;

			var isPathToFile = File.Exists(path);

			var packPath = PackUtilities.GetRootPackUriWithSuffix(path);
			var fullUri = new Uri(isPathToFile ? path : packPath);

			if (isSvg)
			{
				return new DrawingImage(new SvgViewbox { Source = fullUri }.Drawings);
			}
			else
			{
				if (closeFileAfterLoad)
				{
					using var stream = isPathToFile
						? File.OpenRead(path)
						: Application.GetResourceStream(fullUri).Stream;

					var image = new BitmapImage();
					image.BeginInit();
					image.CacheOption = BitmapCacheOption.OnLoad;
					image.StreamSource = stream;
					image.EndInit();

					return image;
				}
				else
				{
					return new BitmapImage(fullUri);
				}
			}
		}

		/// <summary>
		/// Gets image source from its file name.
		/// </summary>
		/// <param name="subfolder">Subfolder in which the image resides (used for <see cref="ImageGetter"/>).</param>
		/// <param name="fileName">File name for the image.</param>
		/// <param name="extension">File extension for the image.</param>
		/// <param name="closeFileAfterLoad">If set to true, <see cref="BitmapImage"/> will be used to load the image from a file
		/// and it will immediately close the file after loading.</param>
		/// <returns>Source to use for <see cref="System.Windows.Controls.Image"/>.</returns>
		public static ImageSource GetFromFileName(string subfolder, string fileName, string extension, bool closeFileAfterLoad)
		{
			if (fileName == null || extension == null) return null;

			var fullFileName = $"{fileName}{PathUtilities.GetExtensionWithDot(extension)}";
			var imagePath = DI.Resources.Get<ImageGetter, string>(subfolder, fullFileName);

			if (imagePath.IsNullOrEmpty()) return null;

			var isSvg = PathUtilities.AreExtensionsEqual(extension, SvgExtension);
			return GetFromPath(imagePath, isSvg, closeFileAfterLoad);
		}

		/// <summary>
		/// Gets image source of an svg image from its file name.
		/// </summary>
		/// <param name="subfolder">Subfolder in which the image resides (used for <see cref="ImageGetter"/>).</param>
		/// <param name="fileName">File name for the image.</param>
		/// <param name="closeFileAfterLoad">If set to true, <see cref="BitmapImage"/> will be used to load the image from a file
		/// and it will immediately close the file after loading.</param>
		/// <returns>Source to use for <see cref="System.Windows.Controls.Image"/>.</returns>
		public static ImageSource GetSvgFromFileName(string subfolder, string fileName, bool closeFileAfterLoad)
			=> GetFromFileName(subfolder, fileName, SvgExtension, closeFileAfterLoad);

		/// <summary>
		/// Gets image source from a static file name getter method.
		/// </summary>
		/// <param name="subfolder">Subfolder in which the image resides (used for <see cref="ImageGetter"/>).</param>
		/// <param name="fileNameGetter">Method used to fetch the file name of the image. 
		/// The method needs to take in an object array and return a string. The following is the format you should pass in:
		/// <para>&lt;static_class_that_contains_the_method&gt;<see cref="FileNameGetterSeparator"/>&lt;method_name&gt;</para>
		/// Example: MyImageGetter.MyMethodThatRetrievesImageFileName where MyImageGetter is a static class and MyMethodThatRetrievesImageFileName
		/// is a method inside of it that returns a string and takes in an object array.</param>
		/// <param name="extension">File extension for the image.</param>
		/// <param name="closeFileAfterLoad">If set to true, <see cref="BitmapImage"/> will be used to load the image from a file
		/// and it will immediately close the file after loading.</param>
		/// <param name="params">Parameters to pass into the getter method.</param>
		/// <returns>Source to use for <see cref="System.Windows.Controls.Image"/>.</returns>
		public static ImageSource GetFromFileNameGetter(string subfolder, string fileNameGetter, string extension, bool closeFileAfterLoad, object[] @params)
		{
			if (fileNameGetter == null || extension == null) return null;

			var parts = fileNameGetter.Split(FileNameGetterSeparator);

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

					if (method?.Invoke(null, new[] { @params }) is string imageFileName)
					{
						return GetFromFileName(subfolder, imageFileName, extension, closeFileAfterLoad);
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets image source of an svg image from a static file name getter method.
		/// </summary>
		/// <param name="subfolder">Subfolder in which the image resides (used for <see cref="ImageGetter"/>).</param>
		/// <param name="fileNameGetter">Method used to fetch the file name of the image. 
		/// The method needs to take in an object array and return a string. The following is the format you should pass in:
		/// <para>&lt;static_class_that_contains_the_method&gt;<see cref="FileNameGetterSeparator"/>&lt;method_name&gt;</para>
		/// Example: MyImageGetter.MyMethodThatRetrievesImageFileName where MyImageGetter is a static class and MyMethodThatRetrievesImageFileName
		/// is a method inside of it that returns a string and takes in an object array.</param>
		/// <param name="closeFileAfterLoad">If set to true, <see cref="BitmapImage"/> will be used to load the image from a file
		/// and it will immediately close the file after loading.</param>
		/// <param name="params">Parameters to pass into the getter method.</param>
		/// <returns>Source to use for <see cref="System.Windows.Controls.Image"/>.</returns>
		public static ImageSource GetSvgFromFileNameGetter(string subfolder, string fileNameGetter, bool closeFileAfterLoad, object[] @params)
			=> GetFromFileNameGetter(subfolder, fileNameGetter, SvgExtension, closeFileAfterLoad, @params);

		/// <summary>
		/// Gets an image source from the information gathered by the parameters.
		/// </summary>
		/// <param name="path">Path to the image file.</param>
		/// <param name="subfolder">Subfolder in which the image resides (used for <see cref="ImageGetter"/>).</param>
		/// <param name="fileName">File name for the image.</param>
		/// <param name="fileNameGetter">Method used to fetch the file name of the image. 
		/// The method needs to take in an object array and return a string. The following is the format you should pass in:
		/// <para>&lt;static_class_that_contains_the_method&gt;<see cref="FileNameGetterSeparator"/>&lt;method_name&gt;</para>
		/// Example: MyImageGetter.MyMethodThatRetrievesImageFileName where MyImageGetter is a static class and MyMethodThatRetrievesImageFileName
		/// is a method inside of it that returns a string and takes in an object array.</param>
		/// <param name="extension">File extension for the image.</param>
		/// <param name="isSvg">Should an svg be retrieved?</param>
		/// <param name="closeFileAfterLoad">If set to true, <see cref="BitmapImage"/> will be used to load the image from a file
		/// and it will immediately close the file after loading.</param>
		/// <param name="paramsGetter">If the file name getter should be used, this defines how should the parameters be provided.</param>
		/// <returns>Source to use for <see cref="System.Windows.Controls.Image"/>.</returns>
		public static ImageSource Get(string path, string subfolder, string fileName, string fileNameGetter, string extension, bool isSvg, bool closeFileAfterLoad, Func<object[]> paramsGetter)
		{
			if(path.IsNotNullOrWhiteSpace())
			{
				var svg = PathUtilities.AreExtensionsEqual(Path.GetExtension(path), SvgExtension);

				return GetFromPath(path, svg, closeFileAfterLoad);
			}
			else if(fileName == null)
			{
				var @params = paramsGetter?.Invoke();

				return isSvg
					? GetSvgFromFileNameGetter(subfolder, fileNameGetter, closeFileAfterLoad, @params)
					: GetFromFileNameGetter(subfolder, fileNameGetter, extension, closeFileAfterLoad, @params);
			}
			else
			{
				return isSvg
					? GetSvgFromFileName(subfolder, fileName, closeFileAfterLoad)
					: GetFromFileName(subfolder, fileName, extension, closeFileAfterLoad);
			}
		}
	}
}
