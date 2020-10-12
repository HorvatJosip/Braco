using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Service used for working with window.
	/// </summary>
	public interface IWindowService
    {
        /// <summary>
        /// Event that triggers once a window is changed.
        /// </summary>
        event EventHandler<WindowChangedEventArgs> WindowChanged;

        /// <summary>
        /// Event that triggers once a page is changed.
        /// </summary>
        event EventHandler<PageChangedEventArgs> PageChanged;

        /// <summary>
        /// Event that triggers once a window has its state changed
        /// to or from maximized.
        /// </summary>
        event EventHandler<MaximizedChangedEventArgs> MaximizedChanged;

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

        /// <summary>
        /// Checks if the window is maximized or not.
        /// </summary>
        /// <param name="window">Window to check.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        /// <returns>True if the given window is maximized, false if it isn't
        /// and null if the window isn't open.</returns>
        bool? IsMaximized(Type window = null);

        /// <summary>
        /// Changes the page to the specified one on the specified window.
        /// </summary>
        /// <param name="data">(optional) Additional data to pass in.</param>
        /// <param name="window">Window on which the page should be changed.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        bool ChangePage<TPage>(object data = null, Type window = null)
			where TPage : PageViewModel;

        /// <summary>
        /// Changes page to the previous one on the specified window.
        /// </summary>
        /// <param name="window">Window on which the page should be changed.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        /// <returns></returns>
        bool GoToPreviousPage(Type window = null);

        /// <summary>
        /// Closes the specified window if it is currently open.
        /// </summary>
        /// <param name="window">Window to close.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        bool Close(Type window = null);

        /// <summary>
        /// Tries to move the specified window on drag.
        /// </summary>
        /// <param name="window">Window to move.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        /// <returns></returns>
        bool DragMove(Type window = null);

        /// <summary>
        /// Centers the given window on the screen.
        /// </summary>
        /// <param name="window">Window to center.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        void Center(Type window = null);

        /// <summary>
        /// Minimizes the specified window.
        /// </summary>
        /// <param name="window">Window to minimize.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        /// <returns></returns>
        bool Minimize(Type window = null);

        /// <summary>
        /// Maximizes or restores the specified window's size.
        /// </summary>
        /// <param name="window">Window to maximize or restore.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        /// <returns>True if maximized, false if restored to normal, null if action didn't go through.</returns>
        bool? MaximizeOrRestore(Type window = null);

        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        /// <param name="window">Window to maximize or restore.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        void Maximize(Type window = null);

        /// <summary>
        /// Restores the specified window to its original size.
        /// </summary>
        /// <param name="window">Window to maximize or restore.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        void Restore(Type window = null);

        /// <summary>
        /// Changes the size of the specified window.
        /// </summary>
        /// <param name="width">Desired width of the window.</param>
        /// <param name="height">Desired height of the window.</param>
        /// <param name="window">Window whose size needs to be changed.
        /// <para>If left null, current window will be used.</para>
        /// </param>
        /// <returns></returns>
        bool ChangeSize(double width, double height, Type window = null);

		/// <summary>
		/// Used for dismissing the current info box.
		/// </summary>
		void DismissInfoBox();

		/// <summary>
		/// Shows info box on the current window.
		/// </summary>
		/// <param name="type">Type of info box.</param>
		/// <param name="title">Title for the info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		void ShowInfoBox(InfoBoxType type, string title, string message, int? duration = null);

		/// <summary>
		/// Shows info box on the current window.
		/// Title is fetched from the type.
		/// </summary>
		/// <param name="type">Type of info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		void ShowInfoBox(InfoBoxType type, string message, int? duration = null);

		/// <summary>
		/// Shows error in the info box on the current window.
		/// </summary>
		/// <param name="title">Title for the info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		void ShowErrorInInfoBox(string title, string message, int? duration = null);

		/// <summary>
		/// Shows error in the info box on the current window.
		/// </summary>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		void ShowErrorInInfoBox(string message, int? duration = null);
	}
}
