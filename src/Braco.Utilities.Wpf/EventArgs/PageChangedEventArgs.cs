using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Event args for <see cref="IWindowService.PageChanged"/> event.
	/// </summary>
	public class PageChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Page that changed.
		/// </summary>
		public Type Page { get; }

		/// <summary>
		/// Window on which the page changed.
		/// </summary>
		public Type Window { get; }

		/// <summary>
		/// Creates an instance of the arguments.
		/// </summary>
		/// <param name="page">Page that changed.</param>
		/// <param name="window">Window on which the page changed.</param>
		public PageChangedEventArgs(Type page, Type window)
		{
			Page = page;
			Window = window;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Changed to page {Page} on window {Window}";
	}
}
