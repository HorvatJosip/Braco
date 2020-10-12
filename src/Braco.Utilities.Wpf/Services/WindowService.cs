using AutoMapper;
using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Default imeplementation of <see cref="IWindowService"/>.
	/// </summary>
	public class WindowService : IWindowService
	{
		/// <summary>
		/// Suffix used for windows.
		/// </summary>
		public const string WindowSuffix = "Window";

		private const int minInfoBoxDuration = 1; // seconds
		private const int infoBoxThreadSleepDuration = 100; // milliseconds

		private readonly ConcurrentQueue<InfoBoxContent> _infoBoxQueue = new ConcurrentQueue<InfoBoxContent>();
		private readonly List<WindowInfo> _windowNavigator = new List<WindowInfo>();
		private readonly IMethodService _methodService;
		private readonly IMapper _mapper;
		private readonly ILocalizer _localizer;
		private readonly IConfigurationService _configuration;

		private InfoBoxContent _currentContent;

		#region Events

		/// <summary>
		/// Event used for singalling that the page changed.
		/// </summary>
		public event EventHandler<PageChangedEventArgs> PageChanged;

		/// <summary>
		/// Event used for singalling that the window changed.
		/// </summary>
		public event EventHandler<WindowChangedEventArgs> WindowChanged;

		/// <summary>
		/// Event used for singalling that the window had its
		/// <see cref="Window.WindowState"/> changed to or
		/// from <see cref="WindowState.Maximized"/>.
		/// </summary>
		public event EventHandler<MaximizedChangedEventArgs> MaximizedChanged;

		#endregion

		/// <summary>
		/// Currently active window or null if there aren't any.
		/// </summary>
		public WindowInfo CurrentWindow => _windowNavigator.Count > 0
			? _windowNavigator.First(info => info.Active)
			: null;

		public WindowViewModel CurrentWindowVM => CurrentWindow?.WindowViewModel;

		/// <summary>
		/// Creates an instance of the service.
		/// </summary>
		/// <param name="methodService">Service used for method invokations.</param>
		/// <param name="mapper">Service for mapping values between objects.</param>
		/// <param name="localizer">Localizer used for the project.</param>
		/// <param name="configuration">Configuration used for the project.</param>
		public WindowService(IMethodService methodService, IMapper mapper, ILocalizer localizer, IConfigurationService configuration)
		{
			_methodService = methodService ?? throw new ArgumentNullException(nameof(methodService));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

			// Start info boxes displaying thread
			Task.Run(DisplayInfoBoxes);
		}

		#region Methods

		/// <inheritdoc/>
		public bool ChangePage<TPage>(object data = null, Type window = null)
			where TPage : PageViewModel
			=> ChangePage(typeof(TPage), data, window);

		/// <summary>
		/// Non generic version of <see cref="ChangePage{TPage}(object, Type)"/>.
		/// </summary>
		/// <param name="page">Page to change to.</param>
		/// <param name="data">(optional) Additional data to pass in.</param>
		/// <param name="window">Window on which the page should be changed.
		/// <para>If left null, current window will be used.</para>
		/// <returns>If the page was changed to the desired one.</returns>
		/// </param>
		public bool ChangePage(Type page, object data, Type window = null)
		{
			if (!typeof(PageViewModel).IsAssignableFrom(page)) return false;

			return ExecuteAction(window, new object[] { page, data });
		}

		/// <inheritdoc/>
		public bool GoToPreviousPage(Type window = null) => ExecuteAction(window);

		/// <inheritdoc/>
		public bool Close(Type window = null) => ExecuteAction(window);

		/// <inheritdoc/>
		public bool DragMove(Type window = null) => ExecuteAction(window);

		/// <inheritdoc/>
		public void Center(Type window = null) => ExecuteAction(window);

		/// <inheritdoc/>
		public bool Minimize(Type window = null) => ExecuteAction(window);

		/// <inheritdoc/>
		public bool? MaximizeOrRestore(Type window = null)
		{
			bool? result = null;

			PerformActionOnWindow(info =>
			{
				info.MaximizeOrRestore();

				result = info.State != WindowState.Normal;

				MaximizedChanged?.Invoke(this, new MaximizedChangedEventArgs(result.Value, info.Window));
			}, window);

			return result;
		}

		/// <inheritdoc/>
		public void Maximize(Type window = null) => PerformActionOnWindow(info =>
		{
			info.Maximize();

			MaximizedChanged?.Invoke(this, new MaximizedChangedEventArgs(true, info.Window));
		});

		/// <inheritdoc/>
		public void Restore(Type window = null) => PerformActionOnWindow(info =>
		{
			info.Restore();

			MaximizedChanged?.Invoke(this, new MaximizedChangedEventArgs(false, info.Window));
		});

		/// <inheritdoc/>
		public bool? IsMaximized(Type window = null)
		{
			bool? result = null;

			PerformActionOnWindow(info => result = info.IsMaximized, window);

			return result;
		}

		/// <inheritdoc/>
		public bool ChangeSize(double width, double height, Type window = null)
			=> ExecuteAction(window, new object[] { width, height });

		/// <inheritdoc/>
		public void DismissInfoBox()
			=> _currentContent?.CancellationTokenSource.Cancel();

		/// <inheritdoc/>
		public void ShowInfoBox(InfoBoxType type, string title, string message, int? duration = null)
			=> _methodService.InvokeOnUIThread(() =>
			{
				// If the title isn't given, use the type of info box as title
				title ??= _localizer[$"{nameof(InfoBoxType)}_{type}_Title"];

				// Get the default duration if it wasn't passed in
				duration ??= _configuration.Get<int>(ResourceKeys.InfoBoxDurationKey);

				// Set duration, but force a minimum of minInfoBoxDuration seconds
				duration = Math.Max(duration.Value, minInfoBoxDuration);

				// Add the message to the queue
				_infoBoxQueue.Enqueue(new InfoBoxContent(title, message, type, duration.Value, false));
			});

		/// <inheritdoc/>
		public void ShowInfoBox(InfoBoxType type, string message, int? duration = null)
			=> ShowInfoBox(type, null, message, duration);

		/// <inheritdoc/>
		public void ShowErrorInInfoBox(string title, string message, int? duration = null)
			=> ShowInfoBox(InfoBoxType.Error, title, message, duration);

		/// <inheritdoc/>
		public void ShowErrorInInfoBox(string message, int? duration = null) 
			=> ShowInfoBox(InfoBoxType.Error, message, duration);

		/// <summary>
		/// Page non generic implementation of <see cref="Open{TWindow, TPage}(OpenWindowOptions)"/>.
		/// </summary>
		/// <typeparam name="TWindow">Window to open.</typeparam>
		/// <param name="page">Page to change to after opening the window.</param>
		/// <param name="options">Options for opening the window.</param>
		/// <returns>If the window was opened.</returns>
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
				// Store for shortness
				var window = typeof(TWindow);

				// Grab the previous window
				var previousWindow = CurrentWindow;

				// Define the result of the action and default it to true
				var success = true;

				// Get the index of wanted window
				var index = _windowNavigator.IndexOf(info => info.Window == window);

				// If it doesn't exist...
				if (index == -1)
				{
					// Allow nulls for previous window action only if it is the first window
					if (_windowNavigator.Count > 0 && options.PreviousWindowAction == null)
						throw new InvalidOperationException("You have to specify what to do with previous window!");

					// Get the info about the window
					var attr = window.GetCustomAttribute<WindowAttribute>();

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
							windowTypeName = window.Name
								// Remove the view model suffix
								.Replace(ContentViewModel.ViewModelSuffix, "");

							// If we don't have a window suffix...
							if (!windowTypeName.EndsWith(WindowSuffix))
								// Add it
								windowTypeName += WindowSuffix;
						}

						// Get the window type by name
						windowType = ReflectionUtilities.FindType(windowTypeName);

						// If it wasn't found...
						if (windowType == null)
							// Throw exception
							throw new ArgumentException($"Window type couldn't be found (searched for {windowTypeName}). Either specify it within the attribute ({nameof(WindowAttribute)}) or make sure that the name of the window follows [WindowName]{WindowSuffix} naming style. ({WindowSuffix} suffix will be added for type search if it isn't already in the name)", nameof(options));
					}

					// Create instance of that window
					var instance = (Window)Activator.CreateInstance(windowType);

					// If there is a previous window
					if (previousWindow != null)
						// Attach the current window to it
						instance.Owner = previousWindow.UI;

					// If the window should stay always on top...
					if (options.StayOnTop)
						// Define it on the instance
						instance.Topmost = true;

					// Create info about the window
					var info = new WindowInfo((sender, page) => PageChanged?.Invoke(sender, new PageChangedEventArgs(page, window)))
					{
						UI = instance,
						Window = window,
						State = instance.WindowState,
						Active = true
					};

					// Whenever the window is activated, set it as the active window
					instance.Activated += (sender, e) =>
					{
						// Set all other windows as inactive
						_windowNavigator.ForEach(i => i.Active = i.Window == window);
					};

					// When the window loads...
					instance.Loaded += (sender, e) =>
					{
						// Find the page holder
						info.PageHolder = ControlTree.FindChild<Frame>(instance);

						// If the size was provided...
						if (options.Size.HasValue)
						{
							// Set it
							ChangeSize(options.Size.Value.width, options.Size.Value.height);

							// If the window should be centered...
							if (options.Center)
								// Center it
								Center();
						}

						// If there are some additional options, add them to view model
						if (options.AdditionalOptions != null && instance.DataContext != null)
							_mapper.Map(options.AdditionalOptions, instance.DataContext, options.AdditionalOptions.GetType(), instance.DataContext.GetType());

						if
						(
							// If the height is bigger than the working height or...
							instance.Height > SystemParameters.WorkArea.Height ||
							// If the width is bigger than the working width...
							instance.Width > SystemParameters.WorkArea.Width
						)
							// Maximize the window
							Maximize(window);

						// Assign the value to window view model
						info.WindowViewModel = instance.DataContext as WindowViewModel;

						// Change to desired page (if specified)
						ChangePage(page, options.PageData, window);

						// Call the on loaded for the window
						info.WindowViewModel.OnLoaded();
					};

					// Listen for when the window closes
					instance.Closed += (sender, e) =>
					{
						// Trigger the on closed method
						info.WindowViewModel.OnClosed();

						// Get the current page view model
						var currentPageVM = info.GetPageViewModel();

						// Trigger the on closed
						options.OnClosed?.Invoke(info.WindowViewModel, currentPageVM);

						// Remove the on closed
						options.OnClosed = null;

						// If there is a page view-model...
						if (currentPageVM != null)
						{
							// Reset the accepted flag
							currentPageVM.Accepted = false;
						}
					};

					// Current window is active, make sure other's don't have the Active flag set
					_windowNavigator.ForEach(i => i.Active = false);

					// Add info to the window navigator
					_windowNavigator.Add(info);

					// Signal the success
					success = true;
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
							_windowNavigator[i].UI.Close();

						// Make the window active
						_windowNavigator.Last().Active = true;
					}
				}

				// If the window was opened...
				if (success)
				{
					// Get currently active window
					var currentWindow = CurrentWindow;

					// Remove from navigator on close
					currentWindow.UI.Closing += (sender, e) => _windowNavigator.Remove(currentWindow);

					// If there is a previous window...
					if (previousWindow != null)
						switch (options.PreviousWindowAction)
						{
							case WindowAction.Hide:
								previousWindow.UI.Hide();
								currentWindow.UI.ShowDialog();
								previousWindow.UI.Show();
								break;
							case WindowAction.Close:
								previousWindow.UI.Hide();
								currentWindow.UI.ShowDialog();
								Close(previousWindow.Window);
								break;
							case WindowAction.LeaveShown:
								currentWindow.UI.ShowDialog();
								break;
							case WindowAction.LeaveShownAndInteractable:
								currentWindow.UI.Show();
								break;
						}

					// Signal that the window has changed
					WindowChanged?.Invoke(this, new WindowChangedEventArgs(window));
				}

				// Return the result
				return success;
			});
		}

		/// <inheritdoc/>
		public bool Open<TWindow, TPage>(OpenWindowOptions options = null)
			where TWindow : WindowViewModel
			where TPage : PageViewModel
		=> Open<TWindow>(typeof(TPage), options);

		/// <inheritdoc/>
		public bool Open<TWindow>(OpenWindowOptions options = null)
			where TWindow : WindowViewModel
		=> Open<TWindow>(null, options);

		private async void DisplayInfoBoxes()
		{
			// Work until the program ends
			while (true)
			{
				// If we have a window and there is an item in the queue...
				if (CurrentWindowVM != null && _infoBoxQueue.TryDequeue(out _currentContent))
				{
					// Setup the properties
					_mapper.Map(_currentContent, CurrentWindowVM.InfoBox);

					try
					{
						// Wait for the duration of the message in seconds
						// and allow for it to be cancelled
						await Task.Delay
						(
							millisecondsDelay: _currentContent.Duration * 1000,
							cancellationToken: _currentContent.CancellationToken
						);
					}
					catch (TaskCanceledException)
					{
						// It was cancelled...
					}

					// After it has finished, dismiss it
					CurrentWindowVM.InfoBox.Dismissed = true;

					// Reset the current content
					_currentContent = null;
				}

				// Make a short pause
				await Task.Delay(infoBoxThreadSleepDuration);
			}
		}

		#region Helpers

		private bool PerformActionOnWindow(Action<WindowInfo> action, Type window = null)
		{
			var info = window == null
				? CurrentWindow
				: _windowNavigator.FirstOrDefault(i => i.Window == window);

			if (info == null)
				return false;

			_methodService.InvokeOnUIThread(() => action(info));
			return true;
		}

		private bool ExecuteAction(Type window, object[] parameters = null, [CallerMemberName] string action = null)
			=> PerformActionOnWindow(info => typeof(WindowInfo).GetMethod(action).Invoke(info, parameters), window);

		#endregion

		#endregion
	}
}
