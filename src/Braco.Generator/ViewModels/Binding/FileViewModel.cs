namespace Braco.Generator
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class FileViewModel
	{
		public string Name { get; set; }
		public string Path { get; set; }

		public override bool Equals(object obj)
			=> obj is FileViewModel vm && Name == vm.Name && Path == vm.Path;
		public override int GetHashCode() => new { Name, Path }.GetHashCode();
		public override string ToString() => $"{Name} ({Path})";

		public static bool operator ==(FileViewModel left, FileViewModel right)
			=> Equals(left, right);
		public static bool operator !=(FileViewModel left, FileViewModel right)
			=> !Equals(left, right);

		public static FileViewModel FromPath(string path)
			=> new FileViewModel
			{
				Path = path,
				Name = System.IO.Path.GetFileNameWithoutExtension(path)
			};

		public static FileViewModel FromFile(System.IO.FileInfo file)
			=> FromPath(file?.FullName);
	}
}
