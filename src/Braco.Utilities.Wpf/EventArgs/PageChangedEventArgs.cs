using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Event args for page changes.
	/// </summary>
	public class PageChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Page that changed.
		/// </summary>
		public PageViewModel Page { get; }

		/// <summary>
		/// Window on which the page changed.
		/// </summary>
		public WindowViewModel Window { get; }

		/// <summary>
		/// Creates an instance of the arguments.
		/// </summary>
		/// <param name="page">Page that changed.</param>
		/// <param name="window">Window on which the page changed.</param>
		public PageChangedEventArgs(PageViewModel page, WindowViewModel window)
		{
			Page = page;
			Window = window;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Changed to page {Page} on window {Window}";
	}
}
