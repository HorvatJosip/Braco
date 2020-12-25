using System;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Exposes methods for a window wrapper.
	/// </summary>
	public interface IWindow
	{
		/// <summary>
		/// View model used for the current window's data context.
		/// </summary>
		WindowViewModel ViewModel { get; }

		/// <summary>
		/// Is this window active or not right now?
		/// </summary>
		bool Active { get; set; }

		/// <summary>
		/// Initial frame manager that was added.
		/// </summary>
		IFrameManager InitialFrameManager { get; }

		/// <summary>
		/// Last frame manager that was added or retrieved.
		/// </summary>
		IFrameManager LastFrameManager { get; }

		/// <summary>
		/// Event used for signalling that the window data changed.
		/// </summary>
		event EventHandler<WindowDataChangedEventArgs> DataChanged;

		/// <summary>
		/// Fired when a page changes.
		/// </summary>
		event EventHandler<PageChangedEventArgs> PageChanged;

		/// <summary>
		/// Maximizes the window.
		/// </summary>
		void Maximize();

		/// <summary>
		/// Minimizes the window.
		/// </summary>
		void Minimize();

		/// <summary>
		/// Restores the window to its original size.
		/// </summary>
		void Restore();

		/// <summary>
		/// Maximizes or restores the window.
		/// </summary>
		void MaximizeOrRestore();

		/// <summary>
		/// Closes the window.
		/// </summary>
		void Close();

		/// <summary>
		/// Tries to drag-move the window.
		/// </summary>
		void DragMove();

		/// <summary>
		/// Centers the window on screen if it isn't maximized.
		/// </summary>
		void Center();

		/// <summary>
		/// Sets the width and height of the window.
		/// </summary>
		void ChangeSize(double width, double height);

		/// <summary>
		/// Adds <see cref="UnorderedFrameManager"/> to the collection of frame managers for this window
		/// in order to manage the given <paramref name="frame"/>.
		/// </summary>
		/// <param name="frame">Frame to manage on this window.</param>
		/// <returns>Instance of the manager.</returns>
		IFrameManager AddFrameManager<TPage>(Frame frame)
			where TPage : PageViewModel;

		/// <summary>
		/// Adds <see cref="IFrameManager"/> to the collection of frame managers for this window.
		/// </summary>
		/// <param name="manager">Frame manager to use for <typeparamref name="TPage"/>.</param>
		/// <returns>Given <paramref name="manager"/>.</returns>
		IFrameManager AddFrameManager<TPage>(IFrameManager manager)
			where TPage : PageViewModel;

		/// <summary>
		/// Gets manager for <typeparamref name="TPage"/>.
		/// </summary>
		/// <typeparam name="TPage">Page type for which the manager is used.</typeparam>
		/// <param name="index">Index of the manager for given <typeparamref name="TPage"/>.
		/// <para>If left null or if it is out of range, first one will be used</para>
		/// </param>
		/// <returns>Manager for <typeparamref name="TPage"/> or null if one doesn't exist.</returns>
		IFrameManager GetFrameManager<TPage>(int? index = null)
			where TPage : PageViewModel;

		/// <summary>
		/// Gets first frame manager that exists.
		/// </summary>
		/// <returns>First manager found, or null if none exist.</returns>
		IFrameManager GetFrameManager();

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
		void ShowInfoBox(InfoBoxType type, string message, int? duration = null)
			=> ShowInfoBox(type, null, message, duration);

		/// <summary>
		/// Shows error in the info box on the current window.
		/// </summary>
		/// <param name="title">Title for the info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		void ShowErrorInInfoBox(string title, string message, int? duration = null)
			=> ShowInfoBox(InfoBoxType.Error, title, message, duration);

		/// <summary>
		/// Shows error in the info box on the current window.
		/// </summary>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		void ShowErrorInInfoBox(string message, int? duration = null)
			=> ShowInfoBox(InfoBoxType.Error, message, duration);

		/// <summary>
		/// Gets data about the window.
		/// </summary>
		/// <returns>Data about the window.</returns>
		WindowData GetData();
	}
}
