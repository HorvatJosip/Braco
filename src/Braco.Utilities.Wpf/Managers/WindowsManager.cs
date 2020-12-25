using AutoMapper;
using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for managing multiple windows.
	/// </summary>
	public class WindowsManager : IWindowsManager
	{
		#region Fields

		private readonly SynchronizedCollection<IWindow> _windowNavigator = new();
		private readonly IMethodService _methodService;
		private readonly IMapper _mapper;

		#endregion

		#region Properties

		/// <inheritdoc/>
		public IWindow ActiveWindow => _windowNavigator.Count > 0
			? _windowNavigator.FirstOrDefault(info => info.Active)
			: null;

		/// <inheritdoc/>
		public IWindow this[Type id] => _windowNavigator.FirstOrDefault(w => w.ViewModel.GetType() == id);

		#endregion

		#region Events

		/// <inheritdoc/>
		public event EventHandler<WindowChangedEventArgs> WindowChanged;

		/// <inheritdoc/>
		public event EventHandler<WindowDataChangedEventArgs> WindowDataChanged;

		/// <inheritdoc/>
		public event EventHandler<PageChangedEventArgs> PageChanged;

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="methodService">Service for invoking methods on UI thread.</param>
		/// <param name="mapper">Service for mapping properties.</param>
		public WindowsManager(IMethodService methodService, IMapper mapper)
		{
			_methodService = methodService ?? throw new ArgumentNullException(nameof(methodService));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		} 

		#endregion

		#region Methods

		/// <summary>
		/// Opens the given <typeparamref name="TWindow"/> with optional
		/// <paramref name="options"/> and <paramref name="page"/>.
		/// </summary>
		/// <typeparam name="TWindow">Window to open.</typeparam>
		/// <param name="page">Page to change to on the window (must inherit from <see cref="PageViewModel"/>).</param>
		/// <param name="options">Options for opening the window.</param>
		/// <returns>If the operation went through successfully.</returns>
		public bool Open<TWindow>(Type page, OpenWindowOptions options = null)
			where TWindow : WindowViewModel
		{
			// If the page was specified, make sure it inherits from page view model
			if (page != null && !typeof(PageViewModel).IsAssignableFrom(page)) return false;

			// Make sure we have open window options
			options ??= new OpenWindowOptions();

			// Make sure to use this on UI thread
			return _methodService.InvokeOnUIThread(() =>
			{
				// Store the type
				var targetWindowType = typeof(TWindow);

				// Grab the previously active window
				var previousWindow = ActiveWindow;

				// Define the result of the action and default it to true
				var success = true;

				// Get the index of wanted window
				var index = _windowNavigator.IndexOf(info => info.ViewModel.GetType() == targetWindowType);

				// If it doesn't exist...
				if (index == -1)
				{
					// Allow nulls for previous window action only if it is the first window
					if (_windowNavigator.Count > 0 && options.PreviousWindowAction == null)
						throw new InvalidOperationException("You have to specify what to do with previous window!");

					// Get the info about the window
					var attr = targetWindowType.GetCustomAttribute<WindowAttribute>();

					// Get the type of window from options
					var windowType = attr?.Type;

					// If it isn't specified...
					if (windowType == null)
					{
						// Get the type name from the attribute
						var windowTypeName = attr?.TypeName;

						// If it isn't provided...
						if (windowTypeName.IsNullOrEmpty())
						{
							// Get the window type name from the type of window view model
							windowTypeName = targetWindowType.Name
								// Remove the view model suffix
								.Replace(ContentViewModel.ViewModelSuffix, "");

							// If we don't have a window suffix...
							if (!windowTypeName.EndsWith(Window.Suffix))
								// Add it
								windowTypeName += Window.Suffix;
						}

						// Get the window type by name
						windowType = ReflectionUtilities.FindType(windowTypeName);

						// If it wasn't found...
						if (windowType == null)
							// Throw exception
							throw new ArgumentException($"Window type couldn't be found (searched for {windowTypeName}). Either specify it within the attribute ({nameof(WindowAttribute)}) or make sure that the name of the window follows [WindowName]{Window.Suffix} naming style. ({Window.Suffix} suffix will be added for type search if it isn't already in the name)", nameof(options));
					}

					// Create instance of that window
					var ui = (System.Windows.Window)Activator.CreateInstance(windowType);

					// If there is a previous window
					if (previousWindow != null)
						// Attach the current window to it
						ui.Owner = (previousWindow as Window).UI;

					// If the window should stay always on top...
					if (options.StayOnTop)
						// Define it on the instance
						ui.Topmost = true;

					// Create a wrapper for the window
					var window = new Window(ui) { Active = true };

					// Subscribe to page changes
					window.PageChanged += (sender, e) => PageChanged?.Invoke(this, e);

					// Whenever the window is activated, set it as the active window
					ui.Activated += (sender, e) =>
					{
						// Set all other windows as inactive
						_windowNavigator.ForEach(w => w.Active = w.ViewModel.GetType() == targetWindowType);
					};

					// When the window loads...
					ui.Loaded += (sender, e) =>
					{
						// If the size was provided...
						if (options.Size.HasValue)
						{
							// Set it
							window.ChangeSize(options.Size.Value.width, options.Size.Value.height);

							// If the window should be centered...
							if (options.Center)
								// Center it
								window.Center();
						}

						// If there are some additional options, add them to view model
						if (options.AdditionalOptions != null && ui.DataContext != null)
							_mapper.Map(options.AdditionalOptions, ui.DataContext, options.AdditionalOptions.GetType(), ui.DataContext.GetType());

						if
						(
							// If the height is bigger than the working height or...
							ui.Height > SystemParameters.WorkArea.Height ||
							// If the width is bigger than the working width...
							ui.Width > SystemParameters.WorkArea.Width
						)
							// Maximize the window
							window.Maximize();

						// Change to desired page (if specified)
						if (typeof(PageViewModel).IsAssignableFrom(page))
						{
							// Add the frame manager to the window
							var frameManager = (IFrameManager)window
								.GetType()
								.GetAMethod(nameof(Window.AddFrameManager), typeof(IFrameManager))
								.MakeGenericMethod(page)
								.Invoke(window, new object[] { new NavigationFrameManager(window.ViewModel, ControlTree.FindChild<Frame>(ui)) });

							// Use it to change the page
							frameManager.ChangePage(page, options.PageData);
						}

						// Call the on loaded for the window
						window.ViewModel.OnLoaded();
					};

					// Listen for when the window closes
					ui.Closed += (sender, e) =>
					{
						// Trigger the on closed method
						window.ViewModel.OnClosed();

						// Trigger the on closed
						options.OnClosed?.Invoke(window.ViewModel);

						// Remove the on closed
						options.OnClosed = null;
					};

					// Current window is active, make sure other's don't have the Active flag set
					_windowNavigator.ForEach(w => w.Active = false);

					// Add info to the window navigator
					_windowNavigator.Add(window);

					// Signal the success
					success = true;

					// Emit data changes here as well
					window.DataChanged += (sender, e) => WindowDataChanged?.Invoke(this, e);
				}

				// Otherwise...
				else
				{
					// Declare where the window removal should start
					var removeStart = index + 1;

					// If it matches the count...
					if (removeStart == _windowNavigator.Count)
						// It is the same as last window, so just bail
						success = false;

					// Otherwise...
					else
					{
						// Clear the windows starting from the end of the list
						for (int i = _windowNavigator.Count - 1; i >= removeStart; i--)
							// Closing will in turn remove them from the navigator
							GetUI(_windowNavigator[i]).Close();

						// Make the window active
						_windowNavigator.Last().Active = true;
					}
				}

				// If the window was opened...
				if (success)
				{
					// Get currently active window
					var currentWindow = ActiveWindow;

					// Remove from navigator on close
					GetUI(currentWindow).Closing += (sender, e) => _windowNavigator.Remove(currentWindow);

					// If there is a previous window...
					if (previousWindow != null)
						switch (options.PreviousWindowAction)
						{
							case WindowAction.Hide:
								GetUI(previousWindow).Hide();
								GetUI(currentWindow).ShowDialog();
								GetUI(previousWindow).Show();
								break;
							case WindowAction.Close:
								GetUI(previousWindow).Hide();
								GetUI(currentWindow).ShowDialog();
								GetUI(previousWindow).Close();
								break;
							case WindowAction.LeaveShown:
								GetUI(currentWindow).ShowDialog();
								break;
							case WindowAction.LeaveShownAndInteractable:
								GetUI(currentWindow).Show();
								break;
						}

					// Signal that the window has changed
					WindowChanged?.Invoke(this, new WindowChangedEventArgs(currentWindow.ViewModel));
				}

				// Return the result
				return success;
			});
		}

		/// <inheritdoc/>
		public bool Open<TWindow>(OpenWindowOptions options = null) where TWindow : WindowViewModel
			=> Open<TWindow>(null, options);

		/// <inheritdoc/>
		public bool Open<TWindow, TPage>(OpenWindowOptions options = null)
			where TWindow : WindowViewModel
			where TPage : PageViewModel
			=> Open<TWindow>(typeof(TPage), options);

		private System.Windows.Window GetUI(IWindow window) => (window as Window).UI;

		#endregion
	}
}
