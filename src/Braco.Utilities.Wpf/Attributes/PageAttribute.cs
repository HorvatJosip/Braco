using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Specifies certain information about a page.
	/// <para>Should be used on a <see cref="PageViewModel"/>.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
    public class PageAttribute : Attribute
    {
		/// <summary>
		/// Subfolders the page is placed in.
		/// </summary>
		public string[] Subfolders { get; }

		/// <summary>
		/// Name of the page.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// If set to true, previous page button will show up (on click, it
		/// executes the <see cref="WindowViewModel.GoToPreviousPageCommand"/>).
		/// </summary>
		public bool AllowGoingToPreviousPage { get; set; }

		/// <summary>
		/// Creates an instance of the attribute which specifies subfolders
		/// that the page is placed in.
		/// </summary>
		/// <param name="subfolders">Subfolders the page is placed in.</param>
		public PageAttribute(params string[] subfolders)
		{
			Subfolders = subfolders;
		}
	}
}
