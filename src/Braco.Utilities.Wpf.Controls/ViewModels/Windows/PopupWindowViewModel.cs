using AutoMapper;

namespace Braco.Utilities.Wpf.Controls.Windows
{
	/// <summary>
	/// Represents data of the popup window.
	/// </summary>
	[Window(typeof(PopupWindow))]
	[AutoMap(sourceType: typeof(DialogContent))]
	public class PopupWindowViewModel : WindowViewModel
	{
		/// <summary>
		/// Title for the popup window.
		/// </summary>
		public string Title { get; set; }
	}
}