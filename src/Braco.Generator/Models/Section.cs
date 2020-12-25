using Braco.Utilities;
using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Braco.Generator
{
	public class Section : IHierarchy<Section>
	{
		public const string DefaultFilesRegex = "*.*";

		public DirectoryInfo Location { get; set; }
		public IList<FileInfo> Files { get; set; } = new List<FileInfo>();
		public string FilesRegex { get; set; } = DefaultFilesRegex;
		public IList<Section> Subsections { get; set; } = new List<Section>();

		public Section(DirectoryInfo location)
		{
			Location = location;
		}

		public Section(DirectoryInfo location, params Section[] subsections) : this(location)
		{
			Subsections = new List<Section>(subsections);
		}

		public IList<FileInfo> ReadFileContents()
		{
			if (Location == null) throw new Exception($"{nameof(Location)} is null...");

			Files = Location.GetFiles(FilesRegex, SearchOption.AllDirectories).ToList();
			return Files;
		}

		public IList<Section> GetChildren() => Subsections;

		public bool IsParentOf(Section item)
			=> Equals(item.Location?.Parent, Location);

		public override bool Equals(object obj)
			=> obj is Section other && other.Location == Location;
		public override int GetHashCode()
			=> new { Location }.GetHashCode();
		public override string ToString()
			=> $"{Location}: {Files.Join(", ")}";

		public static bool operator ==(Section left, Section right)
			=> Equals(left, right);
		public static bool operator !=(Section left, Section right)
			=> !Equals(left, right);
	}
}
