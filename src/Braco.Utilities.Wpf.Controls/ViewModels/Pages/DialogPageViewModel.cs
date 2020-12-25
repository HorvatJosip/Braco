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
		public string Content { get; set; }
		public IEnumerable<ImageButtonViewModel> Buttons { get; set; }
	}
}
