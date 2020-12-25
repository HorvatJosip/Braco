using Braco.Services;
using Braco.Utilities.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Braco.Generator
{
	public class Project
	{
		public FileInfo File { get; set; }
		public DirectoryInfo Directory { get; set; }
		public ProjectFileContent FileContent { get; set; }
		public string Name { get; private set; }

		public Section Constants { get; private set; }
		public Section Localization { get; private set; }
		public Section Pages { get; private set; }
		public Section Fonts { get; private set; }
		public Section Images { get; private set; }

		public Project(FileInfo projectFile)
		{
			File = projectFile ?? throw new ArgumentNullException(nameof(projectFile));

			FileContent ??= System.IO.File.Exists(File.FullName)
				? JsonConvert.DeserializeObject<ProjectFileContent>(System.IO.File.ReadAllText(File.FullName))
				: new ProjectFileContent();

			Directory = File.Directory;

			Name = Path.GetFileNameWithoutExtension(File.FullName);
		}

		public void InitializeSections()
		{
			EnumerateSections(prop =>
			{
				var section = new Section(new DirectoryInfo(Path.Combine(Directory.FullName, prop.Name)));
				if (!section.Location.Exists) section.Location.Create();
				section.ReadFileContents();
				prop.SetValue(this, section);
			});

			DI.Get<ImagesPageViewModel>().Images = new ObservableCollection<PreviewableImageViewModel>
			(
				Images.Files.Select(PreviewableImageViewModel.FromFile)
			);

			DI.Get<FontsPageViewModel>().FontFamilies = new ObservableCollection<FontFamilyViewModel>
			(
				Fonts.Location.GetDirectories().Select
				(
					directory => new FontFamilyViewModel(directory.Name, directory.GetFiles().Select(FontViewModel.FromFile))
				)
			);

			DI.Get<LocalizationPageViewModel>().CultureLocalizations = new ObservableCollection<CultureLocalizationViewModel>
			(
				Localization.Location.GetDirectories().Select
				(
					directory => CultureLocalizationSerializationData.ViewModelFromDirectory(directory)
				)
			);
		}

		public void EnumerateSections(Action<PropertyInfo> action)
			=> GetType()
				.GetProperties()
				.Where(prop => prop.PropertyType == typeof(Section))
				.ForEach(action);

		public void Save()
		{
			System.IO.File.WriteAllText(File.FullName, JsonConvert.SerializeObject(FileContent));
		}

		public override string ToString()
			=> $"{Name} ({File})";
	}
}
