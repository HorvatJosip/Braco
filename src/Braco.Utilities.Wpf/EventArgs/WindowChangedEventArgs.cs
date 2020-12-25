using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Event args for window changes.
	/// </summary>
	public class WindowChangedEventArgs : EventArgs
	{
		/// <summary>
		/// View model of a window that changed.
		/// </summary>
		public WindowViewModel Window { get; }

		/// <summary>
		/// Creates an instance of the arguments.
		/// </summary>
		/// <param name="window">View model of a window that changed.</param>
		public WindowChangedEventArgs(WindowViewModel window)
		{
			Window = window;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Changed to window {Window}";
	}
}
