namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// View model that is a base for page view models.
	/// </summary>
	public class PageViewModel : ContentViewModel
	{
		/// <summary>
		/// Used for specifying the result.
		/// </summary>
		public bool Accepted { get; set; }

		#region Methods

		/// <summary>
		/// Occurrs when the page is loaded.
		/// </summary>
		/// <param name="windowVM">View model of window on which the page loaded.</param>
		/// <param name="pageData">Data used for the page.</param>
		/// <param name="previousPage">Page that was previously active.</param>
		public virtual void OnLoaded(WindowViewModel windowVM, object pageData, PageViewModel previousPage)
		{
			if (pageData != null)
			{
				_mapper.Map(pageData, this, pageData.GetType(), GetType());
			}
		}

		/// <summary>
		/// Occurs when the page is closing.
		/// </summary>
		/// <param name="windowVM">View model of window on which the page is closing.</param>
		public virtual void OnClosing(WindowViewModel windowVM) { }

		#endregion
	}
}
