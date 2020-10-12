using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Event args for <see cref="IWindowService.WindowChanged"/> event.
	/// </summary>
	public class WindowChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Window that changed.
		/// </summary>
		public Type Window { get; }

		/// <summary>
		/// Creates an instance of the arguments.
		/// </summary>
		/// <param name="window">Window that changed.</param>
		public WindowChangedEventArgs(Type window)
		{
			Window = window;
		}
	}
}
