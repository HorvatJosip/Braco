using Braco.Services;
using Braco.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for managing pages on a <see cref="Frame"/> in a way that pages are defined as
	/// they are navigated to and then they can be changed to in no particular order.
	/// <para>Example: user navigated to 5th page. If they then navigate to 2nd page, page 5 becomes "previous page".
	/// If they then go back to previous page, they will go back to the 5th page.</para>
	/// </summary>
	public class UnorderedFrameManager : IFrameManager
	{
		#region Fields

		private object _pageData;

		private readonly Frame _frame;
		private readonly SynchronizedCollection<Type> _pageCollection = new();

		#endregion

		#region Properties

		/// <inheritdoc/>
		public bool CanGoToPreviousPage => _pageCollection.Count > 1;

		/// <inheritdoc/>
		public Type Page => CurrentPageViewModel?.GetType();

		/// <inheritdoc/>
		public PageViewModel PreviousPageViewModel { get; private set; }

		/// <inheritdoc/>
		public PageViewModel CurrentPageViewModel => (_frame?.Content as Page)?.DataContext as PageViewModel;

		/// <inheritdoc/>
		public WindowViewModel WindowViewModel { get; }

		/// <inheritdoc/>
		public IConfigurationService Configuration { get; init; }

		/// <inheritdoc/>
		public IAuthService AuthService { get; init; }

		#endregion

		/// <inheritdoc/>
		public event EventHandler<PageChangedEventArgs> PageChanged;

		#region Constructor

		/// <summary>
		/// Initializes the frame manager.
		/// </summary>
		/// <param name="windowViewModel">View model of the window where the <paramref name="frame"/> resides.</param>
		/// <param name="frame"><see cref="Frame"/> that will be used to host pages.</param>
		/// <param name="initConfigurationFromDI">Determines if <see cref="Configuration"/> should be loaded using <see cref="DI.Get{T}"/>.</param>
		/// <param name="initAuthServiceFromDI">Determines if <see cref="AuthService"/> should be loaded using <see cref="DI.Get{T}"/>.</param>
		public UnorderedFrameManager(WindowViewModel windowViewModel, Frame frame, bool initConfigurationFromDI = true, bool initAuthServiceFromDI = true)
		{
			// Make sure we have a frame to manage the pages for
			_frame = frame ?? throw new ArgumentNullException(nameof(frame));
			// Make sure we have a reference to the window's view model
			WindowViewModel = windowViewModel ?? throw new ArgumentNullException(nameof(windowViewModel));

			// Subscribe to load completed which will fire whenever a page loads
			_frame.LoadCompleted += Frame_LoadCompleted;

			// Initialize services if wanted
			if (initConfigurationFromDI) Configuration = DI.Get<IConfigurationService>();
			if (initAuthServiceFromDI) AuthService = DI.Get<IAuthService>();
		}

		#endregion

		#region Methods

		/// <inheritdoc/>
		public bool GoToPreviousPage(object data = null)
		{
			// Get the previous page's type
			var previousPage = PreviousPageViewModel?.GetType();

			// Make sure it exists
			if (previousPage == null)
				return false;

			// Go to previous page
			ChangePage(previousPage, data);

			// Signal success
			return true;
		}

		/// <inheritdoc/>
		public bool ChangePage(Type page, object data = null)
		{
			// If we don't have the given page...
			if (!_pageCollection.Contains(page))
			{
				// Add it to the collection
				_pageCollection.Add(page);
			}

			// Remember the current page view model for later
			PreviousPageViewModel = CurrentPageViewModel;

			// If the current page exists...
			if (PreviousPageViewModel != null)
			{
				// Trigger the on closing event
				PreviousPageViewModel.OnClosing(WindowViewModel);

				// Reset the accepted flag
				PreviousPageViewModel.Accepted = false;
			}

			// Keep track of the data for when the page loads
			_pageData = data;

			// Provide URI to the new page
			_frame.Source = PageTypeToFrameSourceConverter.Instance.Convert(page, null, null, null) as Uri;

			// Signal success
			return true;
		}

		/// <inheritdoc/>
		public bool ChangePage<TPage>(object data = null) where TPage : PageViewModel
			=> ChangePage(typeof(TPage), data);

		/// <inheritdoc/>
		public bool ContainsPage(Type page)
			=> _pageCollection.Contains(page);

		/// <inheritdoc/>
		public bool ContainsPage<TPage>() where TPage : PageViewModel
			=> ContainsPage(typeof(TPage));

		private void Frame_LoadCompleted(object sender, NavigationEventArgs e)
		{
			// Get the view model for the page that loaded
			var pageVM = CurrentPageViewModel;

			// Make sure it is found
			if (pageVM == null) throw new InvalidOperationException($"Couldn't get view model of the page from the frame (frame content: {_frame?.Content})");

			// Set the manager for target page's view model
			this.InitializeForPage();

			// Signal that the page has changed
			PageChanged?.Invoke(this, new PageChangedEventArgs(pageVM, WindowViewModel));

			// Check if the user is authorized
			this.Authorize();

			// Load settings if there are any
			this.LoadSettings();

			// Call the page's on loaded method
			pageVM.OnLoaded(WindowViewModel, _pageData, PreviousPageViewModel);
		}

		public override bool Equals(object obj)
			=> obj is UnorderedFrameManager other && other.GetHashCode() == GetHashCode();
		public override int GetHashCode()
			=> new { Frame = _frame, Window = WindowViewModel?.GetType() }.GetHashCode();

		#endregion
	}
}
