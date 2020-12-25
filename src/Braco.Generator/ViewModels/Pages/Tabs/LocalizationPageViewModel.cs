using Braco.Utilities;
using Braco.Utilities.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Braco.Generator
{
	[Page("Tabs")]
	public class LocalizationPageViewModel : PageViewModel
	{
		private WindowData _initialWindowData;

		public ObservableCollection<CultureLocalizationViewModel> CultureLocalizations { get; set; }
		public List<CultureInfo> Cultures { get; }
		public CultureInfo SelectedCulture { get; set; }

		public ICommand AddCultureLocalizationCommand { get; set; }
		public ICommand RemoveCultureLocalizationCommand { get; set; }

		public LocalizationPageViewModel()
		{
			Cultures = new List<CultureInfo>(CultureInfo.GetCultures(CultureTypes.AllCultures));

			AddCultureLocalizationCommand = new RelayCommand(OnAddCultureLocalization);
			RemoveCultureLocalizationCommand = new RelayCommand<CultureLocalizationViewModel>(OnRemoveCultureLocalization);
		}

		private void OnAddCultureLocalization()
		{
			CultureLocalizations.Add(new CultureLocalizationViewModel(SelectedCulture));
			
			SelectedCulture = null;
		}

		private void OnRemoveCultureLocalization(CultureLocalizationViewModel culture)
		{
			CultureLocalizations.Remove(culture);
			// TODO: remove directory
		}

		public override void OnLoaded(WindowViewModel windowVM, object pageData, PageViewModel previousPage)
		{
			base.OnLoaded(windowVM, pageData, previousPage);

			_initialWindowData = _windows.ActiveWindow.GetData();
			_windows.ActiveWindow.Maximize();
		}

		public override void OnClosing(WindowViewModel windowVM)
		{
			if(_initialWindowData.State != System.Windows.WindowState.Maximized)
			{
				_windows.ActiveWindow.Restore();
			}
		}
	}
}
