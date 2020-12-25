using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Data commonly used for a window.
	/// </summary>
	public class WindowData
	{
		/// <summary>
		/// State in which the window currently is.
		/// </summary>
		public WindowState State { get; }

		/// <summary>
		/// Size of the window.
		/// </summary>
		public Size Size { get; }

		/// <summary>
		/// Location of the window.
		/// </summary>
		public Point Location { get; }

		/// <summary>
		/// Generates an instance with given data.
		/// </summary>
		/// <param name="state">State in which the window currently is.</param>
		/// <param name="size">Size of the window.</param>
		/// <param name="location">Location of the window.</param>
		public WindowData(WindowState state, Size size, Point location)
		{
			State = state;
			Size = size;
			Location = location;
		}
	}
}
