using Braco.Utilities;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using Braco.Utilities.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Braco.Generator
{
	[Page("Tabs")]
	public class FontsPageViewModel : PageViewModel
	{
		private const string fontFilters = "*.ttf;*.otf;*.woff";

		private readonly IChooserDialogsService _dialogs;

		public ObservableCollection<FontFamilyViewModel> FontFamilies { get; set; }

		public ICommand AddFontFamilyCommand { get; set; }
		public ICommand AddFontsToFamilyCommand { get; set; }
		public ICommand RemoveFontFamilyCommand { get; set; }
		public ICommand RemoveFontFromFamilyCommand { get; set; }

		public FontsPageViewModel(IChooserDialogsService dialogs)
		{
			_dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));

			AddFontFamilyCommand = new RelayCommand(OnAddFontFamily);
			AddFontsToFamilyCommand = new RelayCommand<FontFamilyViewModel>(OnAddFontsToFamily);
			RemoveFontFamilyCommand = new RelayCommand<FontFamilyViewModel>(OnRemoveFontFamily);
			RemoveFontFromFamilyCommand = new RelayCommand<RemoveFontFromFamilyRequest>(OnRemoveFontFromFamily);
		}

		private void OnAddFontFamily()
		{
			PickFonts(fontPaths =>
			{
				var fontFamily = new FontFamilyViewModel();
				var existingPaths = AddFontsToFamily(fontFamily, fontPaths);
				fontFamily.SetNameFromFonts();

				this.Change(x => x.FontFamilies, fontFamilies => fontFamilies.Add(fontFamily));

				if (existingPaths.IsNotNullOrEmpty())
				{
					Dialog.Open(new DialogContent("Some fonts already exist", existingPaths.Join(", ")));
				}
			});
		}

		private void OnAddFontsToFamily(FontFamilyViewModel fontFamily)
		{
			PickFonts(paths =>
			{
				var existingPaths = AddFontsToFamily(fontFamily, paths);

				if (existingPaths.IsNotNullOrEmpty())
				{
					Dialog.Open(new DialogContent("Some fonts already exist", existingPaths.Join(", ")));
				}
			});
		}

		private void OnRemoveFontFamily(FontFamilyViewModel fontFamily)
		{
			if (fontFamily.RemoveAllFonts())
			{
				this.Change(x => x.FontFamilies, fontFamilies => fontFamilies.Remove(fontFamily));
			}
		}

		private void OnRemoveFontFromFamily(RemoveFontFromFamilyRequest request)
		{
			if (request == null) return;

			var (fontFamily, font) = request;

			try
			{
				new[] { false, true }
					.Select(isCSProj => fontFamily.GetTargetFilePath(font.Path, isCSProj))
					.ForEach(fontPath => File.Delete(fontPath));

				fontFamily.Fonts.Remove(font);

				if(fontFamily.Fonts.Count == 0 &&fontFamily.RemoveAllFonts())
				{
					this.Change(x => x.FontFamilies, fontFamilies => fontFamilies.Remove(fontFamily));
				}
			}
			catch (Exception ex)
			{
				Dialog.Open(new DialogContent("Failed to remove font...", $"An error occurred trying to remove the font from its family:{Environment.NewLine}{ex.Message}{Environment.NewLine}({request})"));
			}
		}

		private List<string> AddFontsToFamily(FontFamilyViewModel fontFamily, IEnumerable<string> fontPaths)
		{
			var existingPaths = new List<string>();

			fontPaths.ForEach(fontPath =>
			{
				new[] { false, true }
					.Select(isCSProj => new FileInfo(fontFamily.GetTargetFilePath(fontPath, csProjDirectory: isCSProj)))
					.ForEach((file, i, stopLooping) =>
					{
						if (file.Exists)
						{
							existingPaths.Add(file.FullName);
							stopLooping();
							return;
						}

						file.Directory.Create();

						File.Copy(fontPath, file.FullName);

						if (i == 0)
						{
							fontFamily.Fonts.Add(FontViewModel.FromFile(file));
						}
					});
			});

			return existingPaths;
		}

		private void PickFonts(Action<IList<string>> pathsCallback)
		{
			var paths = _dialogs.ChooseFiles("Pick fonts to use for the font family", ("Font files", fontFilters), ("All files", "*.*"));

			if (paths.IsNotNullOrEmpty()) pathsCallback?.Invoke(paths);
		}
	}
}
