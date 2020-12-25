using Braco.Services.Abstractions;
using System;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for managing pages on a <see cref="Frame"/>.
	/// </summary>
	public interface IFrameManager
	{
		/// <summary>
		/// Defines if the current page allows going to previous page.
		/// </summary>
		bool CanGoToPreviousPage { get; }

		/// <summary>
		/// Page that is currently being displayed.
		/// </summary>
		Type Page { get; }

		/// <summary>
		/// View model of the previous page.
		/// </summary>
		PageViewModel PreviousPageViewModel { get; }

		/// <summary>
		/// View model of the current page, extracted from <see cref="Frame"/>.
		/// </summary>
		PageViewModel CurrentPageViewModel { get; }

		/// <summary>
		/// View model of the window on which the <see cref="Frame"/> resides.
		/// </summary>
		WindowViewModel WindowViewModel { get; }

		/// <summary>
		/// Service used to load and save settings.
		/// </summary>
		IConfigurationService Configuration { get; init; }

		/// <summary>
		/// Service used for checking if user is authorized to access pages marked with <see cref="AuthorizeAttribute"/>.
		/// </summary>
		IAuthService AuthService { get; init; }

		/// <summary>
		/// Fired when a page changes on the <see cref="Frame"/>.
		/// </summary>
		event EventHandler<PageChangedEventArgs> PageChanged;

		/// <summary>
		/// Returns to the previous page (if there are any).
		/// </summary>
		/// <param name="data">Additional data to pass in (optional).</param>
		/// <returns>If the page changed.</returns>
		bool GoToPreviousPage(object data = null);

		/// <summary>
		/// Changes page to the new one.
		/// </summary>
		/// <param name="page">Page to change to.</param>
		/// <param name="data">Additional data to pass in (optional).</param>
		bool ChangePage(Type page, object data = null);

		/// <summary>
		/// Changes page to the new one.
		/// </summary>
		/// <typeparam name="TPage">Page to change to.</typeparam>
		/// <param name="data">Additional data to pass in (optional).</param>
		bool ChangePage<TPage>(object data = null) where TPage : PageViewModel;

		/// <summary>
		/// Checks if the given page exists for this frame manager.
		/// </summary>
		/// <param name="page">Page to check if it exists for this frame manager.</param>
		/// <returns>If this frame manager has the given page.</returns>
		bool ContainsPage(Type page);

		/// <summary>
		/// Checks if the given page exists for this frame manager.
		/// </summary>
		/// <typeparam name="TPage">Page to check if it exists for this frame manager.</typeparam>
		/// <returns>If this frame manager has the given page.</returns>
		bool ContainsPage<TPage>() where TPage : PageViewModel;
	}
}
