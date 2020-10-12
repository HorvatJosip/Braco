using System;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Defines options about opening a window.
    /// </summary>
    public class OpenWindowOptions
    {
        /// <summary>
        /// What should be done with the previously active window.
        /// </summary>
        public WindowAction? PreviousWindowAction { get; set; }

		/// <summary>
		/// Size of the window.
		/// </summary>
		public (double width, double height)? Size { get; set; }

        /// <summary>
        /// Should the opened window be always on top?
        /// </summary>
        public bool StayOnTop { get; set; }

        /// <summary>
        /// Should the window be centered?
        /// <para>Defaults to true.</para>
        /// </summary>
        public bool Center { get; set; } = true;

        /// <summary>
        /// Any required additional options.
        /// </summary>
        public object AdditionalOptions { get; set; }

        /// <summary>
        /// Data that should be passed into the page.
        /// </summary>
        public object PageData { get; set; }

        /// <summary>
        /// Triggered when the window closes.
        /// </summary>
        public Action<WindowViewModel, PageViewModel> OnClosed { get; set; }
    }
}
