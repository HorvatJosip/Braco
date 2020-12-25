using Braco.Utilities;
using Braco.Utilities.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Braco.Generator
{
	public class CultureLocalizationSerializationData
	{
		public const string CultureSeparator = "___";

		public List<CultureLocalizationSectionSerializationData> Sections { get; set; }

		public static CultureLocalizationViewModel ViewModelFromDirectory(DirectoryInfo directory)
		{
			var parts = directory.Name.Split(CultureSeparator);

			if (parts.Length < 2) return null;

			var sections = directory.EnumerateFiles().Select(file =>
			{
				using var fileReader = new StreamReader(file.OpenRead());

				var json = fileReader.ReadToEnd();

				return new LocalizedTableViewModel(Path.GetFileNameWithoutExtension(file.Name), JsonConvert.DeserializeObject<IEnumerable<LocalizedValueViewModel>>(json));
			});

			var viewModel = new CultureLocalizationViewModel(new CultureInfo(parts[1]), sections);

			return viewModel;
		}

		public static (CultureLocalizationSerializationData data, string directoryName) FromCulture(CultureLocalizationViewModel culture)
		{
			var data = new CultureLocalizationSerializationData
			{
				Sections = culture.Sections.Select(section => new CultureLocalizationSectionSerializationData
				{
					Name = section.Name,
					Json = JsonConvert.SerializeObject(section.DataManager.AllItems)
				}).ToList()
			};

			return (data, directoryName: $"{PathUtilities.GetFileNameWithoutInvalidChars(culture.Culture.EnglishName)}{CultureSeparator}{culture.Culture.Name}");
		}
	}
}
