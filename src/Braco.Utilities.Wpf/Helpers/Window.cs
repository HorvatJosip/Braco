using AutoMapper;
using Braco.Services;
using Braco.Services.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Encapsulates the <see cref="System.Windows.Window"/> and handles the common interactions.
	/// </summary>
	public class Window : IWindow
	{
		#region Constants

		/// <summary>
		/// Suffix used for windows.
		/// </summary>
		public const string Suffix = "Window";

		private const int minInfoBoxDuration = 1; // seconds
		private const int infoBoxThreadSleepDuration = 100; // milliseconds 

		#endregion

		#region Fields

		private readonly ConcurrentDictionary<Type, HashSet<IFrameManager>> _frameManagers = new();
		private readonly ConcurrentQueue<InfoBoxContent> _infoBoxQueue = new();

		private bool _showInfoBoxes = true;
		private InfoBoxContent _currentInfoBoxContent;
		private IMethodService _methodService;
		private IMapper _mapper;
		private ILocalizer _localizer;
		private IConfigurationService _configuration;

		#endregion

		#region Properties

		/// <summary>
		/// Actual window that can be displayed.
		/// </summary>
		public System.Windows.Window UI { get; }

		/// <summary>
		/// State of the window.
		/// </summary>
		public WindowState State => UI.WindowState;

		/// <summary>
		/// True if the <see cref="State"/> is set to
		/// <see cref="WindowState.Maximized"/>.
		/// </summary>
		public bool IsMaximized => State == WindowState.Maximized;

		/// <inheritdoc/>
		public WindowViewModel ViewModel { get; }

		/// <inheritdoc/>
		public bool Active { get; set; }

		/// <inheritdoc/>
		public IFrameManager LastFrameManager { get; private set; }

		/// <inheritdoc/>
		public IFrameManager InitialFrameManager { get; private set; }

		#endregion

		#region Events

		/// <inheritdoc/>
		public event EventHandler<WindowDataChangedEventArgs> DataChanged;

		/// <inheritdoc/>
		public event EventHandler<PageChangedEventArgs> PageChanged; 

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance with window's ui representation.
		/// </summary>
		/// <param name="ui">Window's ui representation.</param>
		public Window(System.Windows.Window ui)
		{
			UI = ui ?? throw new ArgumentNullException(nameof(ui));

			if (UI.DataContext is not WindowViewModel windowViewModel)
			{
				throw new InvalidOperationException($"The given window must have a {nameof(WindowViewModel)} for data context.");
			}

			UI.Loaded += (sender, e) =>
			{
				_mapper ??= DI.Mapper;
				_localizer ??= DI.Localizer;
				_configuration ??= DI.Configuration;
				_methodService ??= DI.Get<IMethodService>();

				if (new object[] { _mapper, _localizer, _configuration, _methodService }.All(x => x != null))
				{
					_showInfoBoxes = true;

					Task.Run(InfoBoxThread);
				}
			};
			UI.Closing += (sender, e) => _showInfoBoxes = false;

			ViewModel = windowViewModel;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Maximizes the window.
		/// </summary>
		public void Maximize()
		{
			UI.WindowState = WindowState.Maximized;

			InvokeDataChanged();
		}

		/// <summary>
		/// Minimizes the window.
		/// </summary>
		public void Minimize()
		{
			UI.WindowState = WindowState.Minimized;

			InvokeDataChanged();
		}

		/// <summary>
		/// Restores the window to its original size.
		/// </summary>
		public void Restore()
		{
			UI.WindowState = WindowState.Normal;

			InvokeDataChanged();
		}

		/// <summary>
		/// Maximizes or restores the window.
		/// </summary>
		public void MaximizeOrRestore()
		{
			UI.WindowState = UI.WindowState == WindowState.Normal
				// If the window's state is normal, it should be set to maximized
				? WindowState.Maximized
				// Otherwise, it should be set to normal
				: WindowState.Normal;

			InvokeDataChanged();
		}

		/// <summary>
		/// Closes the window.
		/// </summary>
		public void Close() => UI.Close();

		/// <summary>
		/// Tries to drag-move the window.
		/// </summary>
		public void DragMove()
		{
			try
			{
				UI.DragMove();

				InvokeDataChanged();
			}
			catch { }
		}

		/// <summary>
		/// Centers the window on screen if it isn't maximized.
		/// </summary>
		public void Center()
		{
			if (UI.WindowState != WindowState.Maximized)
			{
				var workArea = SystemParameters.WorkArea;

				UI.Left = (workArea.Width - UI.Width) / 2 + workArea.Left;
				UI.Top = (workArea.Height - UI.Height) / 2 + workArea.Top;

				InvokeDataChanged();
			}
		}

		/// <summary>
		/// Sets the width and height of the window.
		/// </summary>
		public void ChangeSize(double width, double height)
		{
			// Set the width and height of the window
			UI.Width = width;
			UI.Height = height;

			InvokeDataChanged();
		}

		/// <summary>
		/// Adds <see cref="UnorderedFrameManager"/> to the collection of frame managers for this window
		/// in order to manage the given <paramref name="frame"/>.
		/// </summary>
		/// <param name="frame">Frame to manage on this window.</param>
		/// <returns>Instance of the manager.</returns>
		public IFrameManager AddFrameManager<TPage>(Frame frame) where TPage : PageViewModel
		{
			return AddFrameManager(typeof(TPage), new UnorderedFrameManager(ViewModel, frame));
		}

		/// <summary>
		/// Adds <see cref="IFrameManager"/> to the collection of frame managers for this window.
		/// </summary>
		/// <param name="manager">Frame manager to use for <typeparamref name="TPage"/>.</param>
		/// <returns>Given <paramref name="manager"/>.</returns>
		public IFrameManager AddFrameManager<TPage>(IFrameManager manager) where TPage : PageViewModel
		{
			return AddFrameManager(typeof(TPage), manager);
		}

		/// <summary>
		/// Gets manager for <typeparamref name="TPage"/>.
		/// </summary>
		/// <typeparam name="TPage">Page type for which the manager is used.</typeparam>
		/// <param name="index">Index of the manager for given <typeparamref name="TPage"/>.
		/// <para>If left null or if it is out of range, first one will be used</para>
		/// </param>
		/// <returns>Manager for <typeparamref name="TPage"/> or null if one doesn't exist.</returns>
		public IFrameManager GetFrameManager<TPage>(int? index = null) where TPage : PageViewModel
		{
			IFrameManager manager = null;

			if(_frameManagers.TryGetValue(typeof(TPage), out var managers))
			{
				if(index.HasValue && index.Value < managers.Count)
				{
					manager = managers.ElementAt(index.Value);
				}

				if(manager == null && managers.Count > 0)
				{
					manager = managers.First();
				}

				if (manager != null)
				{
					LastFrameManager = manager;
				}
			}

			return manager;
		}

		/// <summary>
		/// Gets first frame manager that exists.
		/// </summary>
		/// <returns>First manager found, or null if none exist.</returns>
		public IFrameManager GetFrameManager()
		{
			foreach (var kvp in _frameManagers)
			{
				if(kvp.Value.Count > 0)
				{
					var manager = kvp.Value.First();

					LastFrameManager = manager;

					return manager;
				}
			}

			return null;
		}

		/// <summary>
		/// Used for dismissing the current info box.
		/// </summary>
		public void DismissInfoBox()
			=> _currentInfoBoxContent?.CancellationTokenSource.Cancel();

		/// <summary>
		/// Shows info box on the current window.
		/// </summary>
		/// <param name="type">Type of info box.</param>
		/// <param name="title">Title for the info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
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

		/// <summary>
		/// Shows info box on the current window.
		/// Title is fetched from the type.
		/// </summary>
		/// <param name="type">Type of info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		public void ShowInfoBox(InfoBoxType type, string message, int? duration = null)
			=> ShowInfoBox(type, null, message, duration);

		/// <summary>
		/// Shows error in the info box on the current window.
		/// </summary>
		/// <param name="title">Title for the info box.</param>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		public void ShowErrorInInfoBox(string title, string message, int? duration = null)
			=> ShowInfoBox(InfoBoxType.Error, title, message, duration);

		/// <summary>
		/// Shows error in the info box on the current window.
		/// </summary>
		/// <param name="message">Message for the info box.</param>
		/// <param name="duration">Duration in seconds until it dismisses.</param>
		public void ShowErrorInInfoBox(string message, int? duration = null)
			=> ShowInfoBox(InfoBoxType.Error, message, duration);

		/// <summary>
		/// Gets data about the window.
		/// </summary>
		/// <returns>Data about the window.</returns>
		public WindowData GetData() => new WindowData
		(
			state: State,
			size: UI.RenderSize,
			location: new Point(UI.Left, UI.Top)
		);

		private async Task InfoBoxThread()
		{
			while (_showInfoBoxes)
			{
				// If we have a window and there is an item in the queue...
				if (ViewModel != null && _infoBoxQueue.TryDequeue(out _currentInfoBoxContent))
				{
					// Setup the properties
					_mapper.Map(_currentInfoBoxContent, ViewModel.InfoBox);

					try
					{
						// Wait for the duration of the message in seconds
						// and allow for it to be cancelled
						await Task.Delay
						(
							millisecondsDelay: (int)(_currentInfoBoxContent.Duration * 1000),
							cancellationToken: _currentInfoBoxContent.CancellationToken
						);
					}
					catch (TaskCanceledException)
					{
						// It was cancelled...
					}

					// After it has finished, dismiss it
					ViewModel.InfoBox.Dismissed = true;

					// Reset the current content
					_currentInfoBoxContent = null;
				}

				// Make a short pause
				await Task.Delay(infoBoxThreadSleepDuration);
			}
		}

		private void InvokeDataChanged()
			=> DataChanged?.Invoke(this, new WindowDataChangedEventArgs(ViewModel, GetData()));

		private IFrameManager AddFrameManager(Type type, IFrameManager manager)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (manager == null) throw new ArgumentNullException(nameof(manager));

			bool added;

			if (!_frameManagers.TryGetValue(type, out var managers))
			{
				added = _frameManagers.TryAdd(type, new HashSet<IFrameManager> { manager });
			}
			else
			{
				added = managers.Add(manager);
			}

			if (added)
			{
				manager.PageChanged += (sender, e) => PageChanged?.Invoke(this, e);
			}

			LastFrameManager = manager;

			InitialFrameManager ??= manager;

			return manager;
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is Window window && window.GetHashCode() == GetHashCode();

		/// <inheritdoc/>
		public override int GetHashCode() => UI.GetHashCode();

		/// <inheritdoc/>
		public override string ToString() => $"{nameof(Window)}: {ViewModel}";

		#endregion
	}
}
