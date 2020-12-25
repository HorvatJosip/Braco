namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// View model that is a base for page view models.
	/// </summary>
	public class PageViewModel : ContentViewModel
	{
		/// <summary>
		/// Frame managers used for this page.
		/// </summary>
		protected IFrameManager _frame;

		/// <summary>
		/// Used for specifying the result.
		/// </summary>
		public bool Accepted { get; set; }

		#region Methods

		/// <summary>
		/// Changes the page to the specified one on the specified window.
		/// </summary>
		/// <param name="data">(optional) Additional data to pass in.</param>
		public bool ChangePage<TPage>(object data = null) where TPage : PageViewModel 
			=> _frame.ChangePage<TPage>(data);

		/// <summary>
		/// Changes page to the previous one on the specified window.
		/// </summary>
		/// <param name="data">(optional) Additional data to pass in.</param>
		public bool GoToPreviousPage(object data = null)
			=> _frame.GoToPreviousPage(data);

		/// <summary>
		/// Occurrs when the page is loaded.
		/// </summary>
		/// <param name="windowVM">View model of window on which the page loaded.</param>
		/// <param name="pageData">Data used for the page.</param>
		/// <param name="previousPage">Page that was previously active.</param>
		public virtual void OnLoaded(WindowViewModel windowVM, object pageData, PageViewModel previousPage)
		{
			if (_mapper != null && pageData != null)
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
