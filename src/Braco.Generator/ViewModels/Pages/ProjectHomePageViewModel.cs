using Braco.Utilities.Wpf;
using System;

namespace Braco.Generator
{
	public class ProjectHomePageViewModel : PageViewModel
	{
		private Project _project;

		public ProjectHomePageTabs Tabs { get; }

		public ProjectHomePageViewModel(ProjectHomePageTabs tabs)
		{
			Tabs = tabs;
			tabs.ConstantsTabClick = OnConstantsTab;
			tabs.FontsTabClick = OnFontsTab;
			tabs.ImagesTabClick = OnImagesTab;
			tabs.LocalizationTabClick = OnLocalizationTab;
			tabs.PagesTabClick = OnPagesTab;
		}

		private void OnConstantsTab()
		{
			ChangePage<ConstantsPageViewModel>();
		}

		private void OnFontsTab()
		{
			ChangePage<FontsPageViewModel>();
		}

		private void OnImagesTab()
		{
			ChangePage<ImagesPageViewModel>();
		}

		private void OnLocalizationTab()
		{
			ChangePage<LocalizationPageViewModel>();
		}

		private void OnPagesTab()
		{
			ChangePage<PagesPageViewModel>();
		}

		public override void OnLoaded(WindowViewModel windowVM, object pageData, PageViewModel previousPage)
		{
			_project = (Project)pageData;

			OnPagesTab();
		}
	}
}
