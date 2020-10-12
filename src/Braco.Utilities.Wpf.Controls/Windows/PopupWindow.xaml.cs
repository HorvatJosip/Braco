using Braco.Services;

namespace Braco.Utilities.Wpf.Controls.Windows
{
	/// <summary>
	/// Interaction logic for PopupWindow.xaml
	/// </summary>
	public partial class PopupWindow : System.Windows.Window
	{
		/// <summary>
		/// Creates an instance of the window.
		/// </summary>
		public PopupWindow()
		{
			WindowHelper.Initialize(this);

			var viewModel = DI.Get<PopupWindowViewModel>();

			DataContext = viewModel;

			if (viewModel != null)
			{
				viewModel.LogoPath = DI.Resources.Get<ImageGetter, string>(ResourceKeys.LogoImage);
			}
		}
	}
}
