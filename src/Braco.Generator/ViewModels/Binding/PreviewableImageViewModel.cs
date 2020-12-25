namespace Braco.Generator
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class PreviewableImageViewModel : FileViewModel
	{
		public bool ShowPreview { get; set; }

		public static new PreviewableImageViewModel FromPath(string path)
			=> new PreviewableImageViewModel
			{
				Path = path,
				Name = System.IO.Path.GetFileNameWithoutExtension(path)
			};

		public static new PreviewableImageViewModel FromFile(System.IO.FileInfo file)
			=> FromPath(file?.FullName);
	}
}
