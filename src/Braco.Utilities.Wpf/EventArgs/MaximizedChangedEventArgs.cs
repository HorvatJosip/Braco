using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Arguments for maximized event on a window.
	/// </summary>
	public class MaximizedChangedEventArgs : EventArgs
	{
		/// <summary>
		/// True if the window is now maximized.
		/// </summary>
		public bool IsMaximized { get; }

		/// <summary>
		/// Window on which the state changed.
		/// </summary>
		public Type Window { get; }

		/// <summary>
		/// Creates an instance of the arguments.
		/// </summary>
		/// <param name="isMaximized">Is the window maximized?</param>
		/// <param name="window">Window that had the maximized state changed.</param>
		public MaximizedChangedEventArgs(bool isMaximized, Type window)
		{
			IsMaximized = isMaximized;
			Window = window;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Window was {(IsMaximized ? "" : "not ")} maximized on {Window}";
	}
}
