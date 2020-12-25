using Braco.Utilities;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace Braco.Generator
{
	public class MainWindowViewModel : Utilities.Wpf.Controls.Windows.MainWindowViewModel
	{
		private readonly ICommand _settingsCommand;

		public MainWindowViewModel()
		{
			_settingsCommand = new RelayCommand(OnSettings);
		}

		private void OnSettings()
		{
			Utilities.Wpf.Controls.Dialog.Open(new Utilities.Wpf.Controls.DialogContent("Yay", "Ha huzjak"));
		}

		protected override void OnLanguageChanged(string culture)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		}

		protected override void OnPageChanged(PageViewModel page)
		{
			SettingsCommand = page.GetType().NotIn(typeof(WelcomePageViewModel), typeof(NewProjectPageViewModel))
				? _settingsCommand
				: null;
		}
	}
}
