using Braco.Services;
using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Contains information about the window and handles the common interactions.
    /// </summary>
    public class WindowInfo
	{
		#region Fields

		private object _pageData;
		private Action<object, Type> _pageChanged;
		private readonly IConfigurationService _configuration;
		private readonly IAuthService _authService;
		private readonly IWindowService _windowService;
		private Frame _pageHolder;

        private readonly List<Type> _pageNavigator = new List<Type>();

		#endregion

		#region Properties

		/// <summary>
		/// Page that is currently being displayed.
		/// </summary>
		public Type Page => _pageNavigator.Last(); // Get the last page from the navigator

		/// <summary>
		/// Window id.
		/// </summary>
		public Type Window { get; set; }

		/// <summary>
		/// Actual window that can be displayed.
		/// </summary>
		public Window UI { get; set; }

		/// <summary>
		/// Frame control that holds the page.
		/// </summary>
		public Frame PageHolder 
		{
			get => _pageHolder;
			set
			{
				// If the value changed...
				if(value != _pageHolder)
				{
					// If the previous one exists...
					if (_pageHolder != null)
						// Remove its event handler
						_pageHolder.LoadCompleted -= PageHolder_LoadCompleted;

					// Assign the new value
					_pageHolder = value;

					// Subscribe to load completed
					value.LoadCompleted += PageHolder_LoadCompleted;
				}
			}
		}

		/// <summary>
		/// View model used for the current window.
		/// </summary>
		public WindowViewModel WindowViewModel { get; set; }

		/// <summary>
		/// Page view model for the previously shown page.
		/// </summary>
		public PageViewModel PreviousPageViewModel { get; private set; }

		/// <summary>
		/// Gets the view model from the <see cref="PageHolder"/>.
		/// </summary>
		public PageViewModel GetPageViewModel()
			=> (PageHolder?.Content as Page)?.DataContext as PageViewModel;

		/// <summary>
		/// State of the window.
		/// </summary>
		public WindowState State { get; set; }

		/// <summary>
		/// True if the <see cref="State"/> is set to
		/// <see cref="WindowState.Maximized"/>.
		/// </summary>
		public bool IsMaximized => State == WindowState.Maximized;

		/// <summary>
		/// Is this window active or not right now?
		/// </summary>
		public bool Active { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of <see cref="WindowInfo"/> 
		/// with page changed event handler.
		/// </summary>
		/// <param name="pageChanged">Method to execute when page changes</param>
		/// <param name="configurationService">Configuration of the project.</param>
		/// <param name="authService">Auth service used for the project.</param>
		/// <param name="windowService">Window service used for the project.</param>
		public WindowInfo(Action<object, Type> pageChanged, IConfigurationService configurationService = null, IAuthService authService = null, IWindowService windowService = null)
		{
			_configuration = configurationService ?? DI.Configuration ?? throw new ArgumentNullException(nameof(configurationService), "Configuration has to be defined.");
			_authService = authService ?? DI.Get<IAuthService>();
			_windowService = windowService ?? DI.Get<IWindowService>() ?? throw new ArgumentNullException(nameof(windowService), "Window service has to be defined.");
			
			_pageChanged = pageChanged;
		}

		#endregion

		#region Methods

		private void ChangeState(WindowState state)
		{
			// Set the state
			State = state;

			// Set the window state of the current window
			UI.WindowState = State;
		}

		/// <summary>
		/// Maximizes the window.
		/// </summary>
		public void Maximize() => ChangeState(WindowState.Maximized);

		/// <summary>
		/// Restores the window to its original size.
		/// </summary>
		public void Restore() => ChangeState(WindowState.Normal);

		/// <summary>
		/// Maximizes or restores the window.
		/// </summary>
		public void MaximizeOrRestore()
			=> ChangeState(
				// If the window's state is normal...
				UI.WindowState == WindowState.Normal
					// It should be set to maximized
					? WindowState.Maximized
					// Otherwise, it should be set to normal
					: WindowState.Normal
			);

		/// <summary>
		/// Returns to the previous page.
		/// </summary>
		public void GoToPreviousPage()
		{
			// Make sure there is a page to go back to
			if (_pageNavigator.Count < 2)
				return;

			// Go to previous page
			ChangePage(_pageNavigator[^2]);
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
			try { UI.DragMove(); }
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
			}
		}

		/// <summary>
		/// Minimizes the window
		/// </summary>
		public void Minimize() => UI.WindowState = WindowState.Minimized;

		/// <summary>
		/// Sets the width and height of the window.
		/// </summary>
		public void ChangeSize(double width, double height)
		{
			// Set the width and height of the current window
			UI.Width = width;
			UI.Height = height;
		}

		/// <summary>
		/// Changes page to the new one.
		/// </summary>
		/// <param name="page">Page to change to.</param>
		/// <param name="data">(optional) Additional data to pass in.</param>
		public void ChangePage(Type page, object data = null)
		{
			// Get the index of wanted page
			var index = _pageNavigator.IndexOf(page);

			// If it doesn't exist...
			if (index == -1)
				// Append it to the end
				_pageNavigator.Add(page);

			// Otherwise...
			else
			{
				// Declare where the page removal should start
				var removeStart = index + 1;

				// If it matches the count...
				if (removeStart == _pageNavigator.Count)
					// It is the same as last page, so just bail
					return;

				// Remove the pages until the wanted one
				_pageNavigator.RemoveRange(removeStart, _pageNavigator.Count - removeStart);
			}

			// If there is a page holder...
			if (PageHolder != null)
			{
				// Remember the current page view model for later
				PreviousPageViewModel = GetPageViewModel();

				// If the current page exists...
				if (PreviousPageViewModel != null)
				{
					// Trigger the on closing event
					PreviousPageViewModel.OnClosing(WindowViewModel);

					// Reset the accepted flag
					PreviousPageViewModel.Accepted = false;
				}
			}

			// Keep track of the data for when the page loads
			_pageData = data;

			// Signal that the page has changed
			_pageChanged?.Invoke(this, page);
		}

		private void PageHolder_LoadCompleted(object sender, NavigationEventArgs e)
		{
			// Get the view model for the page that loaded
			var pageVM = GetPageViewModel();

			// Specify if window can go to previous page
			WindowViewModel.CanGoToPreviousPage = pageVM
				.GetType()
				.GetCustomAttribute<PageAttribute>()?
				.AllowGoingToPreviousPage == true;

			if
			(
				// If the authorize attribute is defined and...
				pageVM.GetType().GetCustomAttribute<AuthorizeAttribute>() != null &&
				// ... the user isn't authenticated...
				_authService?.IsAuthenticated != true
			)
			{
				// Get the defined page view model type for unauthenticated location
				var unauthenticatedPage = DI.Resources.Get<PageGetter, Type>(_authService.UnauthenticatedLocation);

				// If it was found and it is indeed a page view model...
				if (typeof(PageViewModel).IsAssignableFrom(unauthenticatedPage))
					// Redirect to it
					typeof(IWindowService)
						.GetAMethod(nameof(IWindowService.ChangePage), typeof(object), typeof(Type))
						.MakeGenericMethod(unauthenticatedPage)?
						.Invoke(_windowService, new object[] { null, null });

				else
					throw new Exception($"Couldn't find page to redirect to for unauthenticated user... Make sure to override {nameof(PageGetter.GetPageType)} method inside {nameof(PageGetter)} and return type that is a {nameof(PageViewModel)}");
			}

			// Otherwise...
			else
			{
				// Get the properties that are declared as settings
				var properties = pageVM
					.GetType()
					.GetProperties()
					.Where(prop => Attribute.IsDefined(prop, typeof(SettingAttribute)));

				// Loop through settings properties
				properties.ForEach(prop =>
				{
					// Fetch the attribute
					var attr = prop.GetCustomAttribute<SettingAttribute>();

					// Get the key for configuration
					var configKey = attr.Key ?? prop.Name;

					// If it should be loaded from settings...
					if (attr.Load && prop.GetSetMethod()?.IsPublic == true)
					{
						// Extract the value from the configuration
						var configValue = _configuration[configKey];

						// If there is a value for it...
						if(configValue != null)
						{
							// Set it
							prop.SetValue(pageVM, prop.PropertyType == typeof(string) 
								? configValue
								: configValue.Convert(prop.PropertyType));
						}
					}

					// If the setting should be updated whenever the property changes...
					if (attr.UpdateOnValueChanged)
						// Subscribe to property changes and set the setting on change
						ReflectionUtilities.ListenForPropertyChanges(pageVM, prop.Name, _ => _configuration.Set(configKey, prop.GetValue(pageVM)));
				});

				// Call the page's on loaded method
				pageVM.OnLoaded(WindowViewModel, _pageData, PreviousPageViewModel);
			}
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) => obj is WindowInfo info && info.Window == Window;
		/// <inheritdoc/>
		public override int GetHashCode() => Window.GetHashCode();

		/// <inheritdoc/>
		public override string ToString()
			=> $"Window: {Window}, Page: {Page}";

		#endregion
	}
}
