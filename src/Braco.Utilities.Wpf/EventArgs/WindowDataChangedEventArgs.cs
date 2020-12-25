using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Event args for <see cref="IWindowsManager.WindowDataChanged"/> event.
	/// </summary>
	public class WindowDataChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Window whose data changed.
		/// </summary>
		public WindowViewModel Window { get; set; }

		/// <summary>
		/// Data for the window.
		/// </summary>
		public WindowData Data { get; set; }

		/// <summary>
		/// Generates an instance with given data.
		/// </summary>
		/// <param name="window">Window whose data changed.</param>
		/// <param name="data">Data for the window.</param>
		public WindowDataChangedEventArgs(WindowViewModel window, WindowData data)
		{
			Window = window;
			Data = data;
		}
	}
}
