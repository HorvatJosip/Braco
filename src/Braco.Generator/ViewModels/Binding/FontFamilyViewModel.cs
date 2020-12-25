using Braco.Services;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Braco.Generator
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class FontFamilyViewModel
	{
		public const string UnnamedFontFamilyDirectoryName = "Unnamed";

		private readonly ProjectManager _projectManager;

		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				if (Equals(value, _name)) return;

				if (TransferFontsToNewDirectory(_name, value))
				{
					_name = value;
				}
			}
		}

		public ObservableCollection<FontViewModel> Fonts { get; }

		public FontFamilyViewModel() : this(string.Empty, null) { }

		public FontFamilyViewModel(string name, IEnumerable<FontViewModel> fonts)
		{
			_projectManager = DI.Get<ProjectManager>();

			Name = name;
			Fonts = fonts == null
				? new ObservableCollection<FontViewModel>()
				: new ObservableCollection<FontViewModel>(fonts);

			if (Name.IsNullOrEmpty()) SetNameFromFonts();
		}

		public string GetTargetFilePath(string fontPath, bool csProjDirectory)
			=> Path.Combine
			(
				csProjDirectory
					? _projectManager.CurrentProject.FileContent.FontsTargetPath
					: _projectManager.CurrentProject.Fonts.Location.FullName,
				Name.ReplaceIfNullOrEmpty(UnnamedFontFamilyDirectoryName),
				Path.GetFileName(fontPath)
			);

		private string GetDirectoryPath(string name, bool csProjDirectory)
			=> Path.Combine
			(
				csProjDirectory
					? _projectManager.CurrentProject.FileContent.FontsTargetPath
					: _projectManager.CurrentProject.Fonts.Location.FullName,
				name.ReplaceIfNullOrEmpty(UnnamedFontFamilyDirectoryName)
			);

		public bool RemoveAllFonts()
		{
			var success = true;

			Fonts.ForEach((font, _, stopLooping) =>
			{
				if (new[] { false, true }.Any
				(
					isCSProj => !RemoveFont(GetTargetFilePath(font.Path, csProjDirectory: isCSProj))
				))
				{
					success = false;
					stopLooping();
				}
			});

			if (success)
			{
				new[] { false, true }
					.Select(isCSProj => GetDirectoryPath(Name.ReplaceIfNullOrEmpty(UnnamedFontFamilyDirectoryName), csProjDirectory: isCSProj))
					.ForEach(dir => Directory.Delete(dir, recursive: true));
			}

			return success;
		}

		public bool RemoveFont(string fontPath)
		{
			if (!File.Exists(fontPath)) return true;

			try
			{
				File.Delete(fontPath);
				return true;
			}
			catch (Exception ex)
			{
				Dialog.Open(new DialogContent("Failed to remove font...", $"An error occurred trying to remove the font file:{Environment.NewLine}{ex.Message}"));
				return false;
			}
		}

		private bool TransferFontsToNewDirectory(string previousName, string newName)
		{
			try
			{
				Fonts.ForEach(font =>
				{
					new[] { false, true }
						.Select(isCSProj => 
						(
							GetDirectoryPath(previousName.ReplaceIfNullOrEmpty(UnnamedFontFamilyDirectoryName), csProjDirectory: isCSProj),
							GetDirectoryPath(newName, csProjDirectory: isCSProj))
						)
						.ForEach(tuple =>
						{
							var (prevDir, newDir) = tuple;

							if (!Directory.Exists(prevDir)) return;

							Directory.CreateDirectory(newDir);

							Directory.GetFiles(prevDir).ForEach(file =>
							{
								File.Move(file, Path.Combine(newDir, Path.GetFileName(file)));
							});

							Directory.Delete(prevDir);
						});
				});

				return true;
			}
			catch (Exception ex)
			{
				Dialog.Open(new DialogContent("Failed to transfer fonts...", $"An error occurred trying to transfer the font files to new directory:{Environment.NewLine}{ex.Message}"));
				return false;
			}
		}

		public void SetNameFromFonts()
		{
			if (Fonts.IsNullOrEmpty()) return;

			var names = Fonts.Select(font => font.Name).ToList();
			var minNameLength = names.Min(name => name.Length);
			var name = new StringBuilder();

			for (int i = 0; i < minNameLength; i++)
			{
				var matches = 1;
				char? current = null;

				for (int j = 0; j < names.Count; j++)
				{
					if (current.HasValue)
					{
						if (names[j][i] == current.Value)
						{
							matches++;
						}
					}
					else
					{
						current = names[j][i];
					}
				}

				if (matches == names.Count && current.HasValue)
				{
					name.Append(current.Value);
				}
			}

			if (name.Length > 0)
			{
				for (int i = 0; i < name.Length; i++)
				{
					if (char.IsLetterOrDigit(name[i])) break;

					name.Remove(i, 1);
					i--;
				}
				for (int i = name.Length - 1; i >= 0; i--)
				{
					if (char.IsLetterOrDigit(name[i])) break;

					name.Remove(i, 1);
				}

				Name = name.ToString();
			}
		}

		public override bool Equals(object obj)
			=> obj is FontFamilyViewModel vm && Name == vm.Name && Enumerable.SequenceEqual(Fonts, vm.Fonts);
		public override int GetHashCode() => new { Name, Fonts }.GetHashCode();
		public override string ToString() => $"{Name} ({Fonts.Join(", ")})";

		public static bool operator ==(FontFamilyViewModel left, FontFamilyViewModel right)
			=> Equals(left, right);
		public static bool operator !=(FontFamilyViewModel left, FontFamilyViewModel right)
			=> !Equals(left, right);
	}
}
