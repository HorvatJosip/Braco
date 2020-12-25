using AutoMapper;
using System.Collections.Generic;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Represents a page used for a dialog.
	/// </summary>
	[AutoMap(sourceType: typeof(DialogContent))]
	public class DialogPageViewModel : PageViewModel
	{
		/// <summary>
		/// Content of the dialog.
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Buttons to use for the dialog.
		/// </summary>
		public IEnumerable<ImageButtonViewModel> Buttons { get; set; }
	}
}
