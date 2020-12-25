using System.Collections.Generic;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// View model that represents a menu and its items.
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class MenuViewModel
	{
		/// <summary>
		/// Header of the menu.
		/// </summary>
		public string Header { get; set; }

		/// <summary>
		/// Buttons that the menu contains.
		/// </summary>
		public IEnumerable<ImageButtonViewModel> Items { get; set; }

		/// <summary>
		/// Creates an empty instance of the view-model.
		/// </summary>
		public MenuViewModel() { }

		/// <summary>
		/// Creates an empty instance with the given header and items.
		/// </summary>
		/// <param name="header">Header of the menu.</param>
		/// <param name="items">Items that the menu contains.</param>
		public MenuViewModel(string header, IEnumerable<ImageButtonViewModel> items)
		{
			Header = header;
			Items = items;
		}

		/// <summary>
		/// Creates an empty instance with the given header and items.
		/// </summary>
		/// <param name="header">Header of the menu.</param>
		/// <param name="items">Items that the menu contains.</param>
		public MenuViewModel(string header, params ImageButtonViewModel[] items)
			: this(header, (IEnumerable<ImageButtonViewModel>)items) { }
	}
}
