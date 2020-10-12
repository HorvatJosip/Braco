namespace Braco.Utilities.Wpf.Controls.Windows
{
	/// <summary>
	/// Represents data of the popup window.
	/// </summary>
	[Window(typeof(PopupWindow))]
	public class PopupWindowViewModel : WindowViewModel
	{
		/// <summary>
		/// Title for the popup window.
		/// </summary>
		public string Title { get; set; }
	}
}