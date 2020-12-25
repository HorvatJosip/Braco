using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Service used for working with windows.
	/// </summary>
	public interface IWindowsManager
	{
		/// <summary>
		/// Currently active window or null if there aren't any.
		/// </summary>
		IWindow ActiveWindow { get; }

		/// <summary>
		/// Tries to find a window whose view model has a type equal to <paramref name="id"/>.
		/// </summary>
		/// <param name="id">View model type of window to find.</param>
		/// <returns><see cref="Window"/> for the given <paramref name="id"/> or null if not found.</returns>
		IWindow this[Type id] { get; }

		/// <summary>
		/// Event used for singalling that the window changed.
		/// </summary>
		event EventHandler<WindowChangedEventArgs> WindowChanged;

		/// <summary>
		/// Event used for signalling that the window data changed.
		/// </summary>
		event EventHandler<WindowDataChangedEventArgs> WindowDataChanged;

		/// <summary>
		/// Fired when a page changes.
		/// </summary>
		event EventHandler<PageChangedEventArgs> PageChanged;

		/// <summary>
		/// Opens a window if it isn't already open.
		/// </summary>
		/// <param name="options">Options for opening the window.</param>
		bool Open<TWindow>(OpenWindowOptions options = null) where TWindow : WindowViewModel;

		/// <summary>
		/// Opens a window if it isn't already open and sets its page.
		/// </summary>
		/// <param name="options">Options for opening the window.</param>
		bool Open<TWindow, TPage>(OpenWindowOptions options = null)
			where TWindow : WindowViewModel
			where TPage : PageViewModel;
	}
}
