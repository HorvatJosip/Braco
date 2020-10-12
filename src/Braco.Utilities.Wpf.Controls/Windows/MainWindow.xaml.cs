using Braco.Services;

namespace Braco.Utilities.Wpf.Controls.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : System.Windows.Window
	{
		/// <summary>
		/// Creates an instance of the window.
		/// </summary>
		public MainWindow()
		{
			WindowHelper.Initialize(this);
			
			var viewModel = DI.Get<MainWindowViewModel>();

			DataContext = viewModel;

			if (viewModel != null)
			{
				viewModel.LogoPath = DI.Resources.Get<ImageGetter, string>(ResourceKeys.LogoImage);
			}
		}
	}
}
