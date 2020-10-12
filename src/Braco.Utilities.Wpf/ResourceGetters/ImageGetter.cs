using Braco.Services.Abstractions;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// <see cref="ResourceGetter"/> for images.
	/// </summary>
	public class ImageGetter : ResourceGetter
	{
		/// <summary>
		/// Default location for images.
		/// </summary>
		public const string DefaultLocation = "Images";

		/// <summary>
		/// Creates the instance of the getter with given location.
		/// </summary>
		/// <param name="location">Location where the images are stored.</param>
		public ImageGetter(string location = DefaultLocation) : base(location)
		{

		}

		/// <summary>
		/// Used for getting the image path.
		/// </summary>
		/// <param name="subfolder">Subfolder of the image.</param>
		/// <param name="fileName">Image's file name.</param>
		/// <returns>Path to the image resource.</returns>
		public virtual string GetImagePath(string subfolder, string fileName)
			=> $"{Location}{(subfolder == null ? "" : $"/{subfolder}")}/{fileName}";

		/// <summary>
		/// Used for getting the image path.
		/// </summary>
		/// <param name="fileName">Image's file name.</param>
		/// <returns>Path to the image resource.</returns>
		public virtual string GetImagePath(string fileName)
			=> GetImagePath(null, fileName);
	}
}