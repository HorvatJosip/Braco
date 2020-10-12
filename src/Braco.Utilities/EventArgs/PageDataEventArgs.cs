namespace Braco.Utilities
{
	/// <summary>
	/// Event arguments for page events.
	/// </summary>
    public class PageDataEventArgs : System.EventArgs
    {
		/// <summary>
		/// Page ordinal number.
		/// </summary>
        public int Page { get; }

		/// <summary>
		/// Number of rows to be displayed on the page.
		/// </summary>
        public int PageSize { get; }

		/// <summary>
		/// Total number of pages that exist.
		/// </summary>
        public int NumPages { get; }

		/// <summary>
		/// Creates an instance of event arguments for page events.
		/// </summary>
		/// <param name="page">page ordinal number.</param>
		/// <param name="pageSize">number of rows to be displayed on the page.</param>
		/// <param name="numPages">total number of pages that exist.</param>
		public PageDataEventArgs(int page, int pageSize, int numPages)
        {
            Page = page;
            PageSize = pageSize;
            NumPages = numPages;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Page {Page} / {NumPages} (Rows per page: {PageSize})";
	}
}
