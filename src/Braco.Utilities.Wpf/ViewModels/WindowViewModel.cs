using System;
using System.Windows;
using System.Windows.Input;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// View model that is a base for window view models.
	/// </summary>
	public class WindowViewModel : ContentViewModel
	{
		#region Properties

		/// <summary>
		/// Page that is active on this window.
		/// </summary>
		public Type Page { get; set; }

		/// <summary>
		/// Path to the logo image.
		/// </summary>
		public string LogoPath { get; set; }

		/// <summary>
		/// Flag that indicates if the window is currently in <see cref="WindowState.Maximized"/>.
		/// </summary>
		public bool IsMaximized { get; set; }

		/// <summary>
		/// Used for setting up and triggering the info box.
		/// </summary>
		public InfoBoxContent InfoBox { get; } = new InfoBoxContent();

		/// <summary>
		/// Flag that indicates if the user can go to previous page or not.
		/// </summary>
		public bool CanGoToPreviousPage { get; set; }

		/// <summary>
		/// Defines the resize mode for the window.
		/// </summary>
		public ResizeMode ResizeMode { get; set; }

		/// <summary>
		/// Size of the previous page button.
		/// </summary>
		public string PreviousPageButtonSize { get; set; } = "35 x 35";

		#endregion

		#region Commands

		/// <summary>
		/// Command that will fire when the settings button is clicked.
		/// </summary>
		public ICommand SettingsCommand { get; protected set; }
		/// <summary>
		/// Command that will fire when the window is set to <see cref="WindowState.Minimized"/>.
		/// </summary>
		public ICommand MinimizeCommand { get; protected set; }
		/// <summary>
		/// Command that will fire when the window is set to <see cref="WindowState.Maximized"/>.
		/// </summary>
		public ICommand MaximizeCommand { get; protected set; }
		/// <summary>
		/// Command that will fire when the window is closed.
		/// </summary>
		public ICommand CloseCommand { get; protected set; }
		/// <summary>
		/// Command that will fire when the window is being dragged.
		/// </summary>
		public ICommand DragCommand { get; protected set; }
		/// <summary>
		/// Command that will fire when the info box is dismissed.
		/// </summary>
		public ICommand DismissInfoBoxCommand { get; protected set; }
		/// <summary>
		/// Command that will fire when the previous page button is clicked.
		/// </summary>
		public ICommand GoToPreviousPageCommand { get; protected set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of the view model.
		/// </summary>
		public WindowViewModel()
		{
			// Listen for page changes on the current window
			_windowService.PageChanged += (sender, e) =>
			{
				if (e.Window == GetType())
					Page = e.Page;
			};

			// Subscribe to maximized changes
			_windowService.MaximizedChanged += (sender, e) =>
			{
				if (e.Window == GetType())
					IsMaximized = e.IsMaximized;
			};

			// Setup the window commands
			MinimizeCommand = new RelayCommand(() => _windowService.Minimize());
			MaximizeCommand = new RelayCommand(() => _windowService.MaximizeOrRestore());
			CloseCommand = new RelayCommand(() => _windowService.Close());
			DragCommand = new RelayCommand(() => _windowService.DragMove());
			DismissInfoBoxCommand = new RelayCommand(() => _windowService.DismissInfoBox());
			GoToPreviousPageCommand = new RelayCommand(() => _windowService.GoToPreviousPage());
		}

		#endregion

		#region Methods

		/// <summary>
		/// Occurs when the window loads.
		/// </summary>
		public virtual void OnLoaded() { }
		/// <summary>
		/// Occurs when the window gets closed.
		/// </summary>
		public virtual void OnClosed() { }

		#endregion
	}
}
