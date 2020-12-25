using Newtonsoft.Json;
using System.IO;

namespace Braco.Generator
{
	public class ProjectFileContent
	{
		public string CsprojPath { get; set; }
		public string ConstantsTargetPath { get; set; }
		public string FontsTargetPath { get; set; }
		public string ImagesTargetPath { get; set; }
		public string LocalizationTargetPath { get; set; }
		public string PagesTargetPath { get; set; }
		public string PageViewModelsTargetPath { get; set; }

		[JsonIgnore]
		public FileInfo CsprojFile => new FileInfo(CsprojPath);
		[JsonIgnore]
		public DirectoryInfo CsprojDirectory => CsprojFile.Directory;

		public void SetDefaultTargetPaths()
		{
			if (CsprojPath == null) return;

			ConstantsTargetPath = Path.Combine(CsprojDirectory.FullName, "Constants");
			FontsTargetPath = Path.Combine(CsprojDirectory.FullName, "Resources", "Fonts");
			ImagesTargetPath = Path.Combine(CsprojDirectory.FullName, "Resources", "Images");
			LocalizationTargetPath = Path.Combine(CsprojDirectory.FullName, "Resources", "Localization");
			PagesTargetPath = Path.Combine(CsprojDirectory.FullName, "Pages");
			PageViewModelsTargetPath = Path.Combine(CsprojDirectory.FullName, "ViewModels", "Pages");
		}
	}
}
