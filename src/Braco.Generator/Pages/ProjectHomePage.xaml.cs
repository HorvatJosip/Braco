using Braco.Utilities.Wpf;

namespace Braco.Generator
{
	/// <summary>
	/// Interaction logic for ProjectHomePage.xaml
	/// </summary>
	public partial class ProjectHomePage : BasePage<ProjectHomePageViewModel>
	{
		public ProjectHomePage()
		{
			InitializeComponent();

			(Services.DI.Get<IWindowsManager>() as WindowsManager).ActiveWindow.AddFrameManager<ProjectHomePageViewModel>(Frame);
		}
	}
}
