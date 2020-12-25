namespace Braco.Generator
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class FontViewModel
	{
		public string Name { get; set; }
		public string Path { get; set; }

		public override bool Equals(object obj)
			=> obj is FontViewModel vm && Name == vm.Name && Path == vm.Path;
		public override int GetHashCode() => new { Name, Path }.GetHashCode();
		public override string ToString() => $"{Name} ({Path})";

		public static bool operator ==(FontViewModel left, FontViewModel right)
			=> Equals(left, right);
		public static bool operator !=(FontViewModel left, FontViewModel right)
			=> !Equals(left, right);

		public static FontViewModel FromPath(string path)
			=> new FontViewModel
			{
				Path = path,
				Name = System.IO.Path.GetFileNameWithoutExtension(path)
			};

		public static FontViewModel FromFile(System.IO.FileInfo file)
			=> FromPath(file?.FullName);
	}
}
