using Braco.Services;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf.Controls.Windows;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Helper methods for using custom dialogs.
	/// </summary>
	public static class Dialog
	{
		private static readonly MethodInfo _openMethod;
		private static readonly PopupWindowViewModel _popupWindowViewModel;
		private static readonly IWindowsManager _windows;

		static Dialog()
		{
			_windows = DI.Get<IWindowsManager>();
			if (_windows == null) throw new NotSupportedException("Windows manager wasn't found...");

			_popupWindowViewModel = DI.Get<PopupWindowViewModel>();
			if (_popupWindowViewModel == null) throw new NotSupportedException($"You must register a type for {nameof(PopupWindowViewModel)}...");

			_openMethod = _windows.GetType().GetAMethod(nameof(IWindowsManager.Open), (m, p) => m.GetGenericArguments().Length == 2 && p.Length == 1 && p[0].ParameterType == typeof(OpenWindowOptions));
		}

		/// <summary>
		/// Opens a dialog with given content.
		/// </summary>
		/// <typeparam name="TPage">Dialog page to open.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		public static object Open<TPage>(DialogContent content)
			where TPage : DialogPageViewModel
			=> ExecuteOpen(DI.Get<TPage>(), content, null);

		/// <summary>
		/// Opens a dialog with given content on a page registered as <see cref="DialogPageViewModel"/>.
		/// </summary>
		/// <param name="content">Content to show on the dialog.</param>
		public static object Open(DialogContent content)
			=> ExecuteOpen(DI.Get<DialogPageViewModel>(), content, null);

		/// <summary>
		/// Opens a dialog with given content.
		/// </summary>
		/// <typeparam name="TPage">Dialog page to open.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="onClosed">Method to execute once the dialog closes.</param>
		public static object Open<TPage>(DialogContent content, Action<WindowViewModel> onClosed)
			where TPage : DialogPageViewModel
			=> ExecuteOpen(DI.Get<TPage>(), content, options => options.OnClosed = onClosed);

		/// <summary>
		/// Opens a dialog with given content on a page registered as <see cref="DialogPageViewModel"/>.
		/// </summary>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="onClosed">Method to execute once the dialog closes.</param>
		public static object Open(DialogContent content, Action<WindowViewModel> onClosed)
			=> ExecuteOpen(DI.Get<DialogPageViewModel>(), content, options => options.OnClosed = onClosed);

		/// <summary>
		/// Opens a dialog with given content and window open options.
		/// </summary>
		/// <typeparam name="TPage">Dialog page to open.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="openOptionsSetup">Used for setting up window opening options.</param>
		public static object Open<TPage>(DialogContent content, Action<OpenWindowOptions> openOptionsSetup)
			where TPage : DialogPageViewModel
			=> ExecuteOpen(DI.Get<TPage>(), content, openOptionsSetup);

		/// <summary>
		/// Opens a dialog with given content and window open options.
		/// </summary>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="openOptionsSetup">Used for setting up window opening options.</param>
		public static object Open(DialogContent content, Action<OpenWindowOptions> openOptionsSetup)
			=> ExecuteOpen(DI.Get<DialogPageViewModel>(), content, openOptionsSetup);

		/// <summary>
		/// Opens a dialog with given content.
		/// </summary>
		/// <typeparam name="TPage">Dialog page to open.</typeparam>
		/// <typeparam name="TResult">Type of object the dialog will return.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="fallbackResult">Result to return if not set.</param>
		public static TResult ForResult<TPage, TResult>(DialogContent content, TResult fallbackResult = default)
			where TPage : DialogPageViewModel
			=> (TResult) ExecuteOpen(DI.Get<TPage>(), content, null, fallbackResult);

		/// <summary>
		/// Opens a dialog with given content on a page registered as <see cref="DialogPageViewModel"/>.
		/// </summary>
		/// <typeparam name="TResult">Type of object the dialog will return.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="fallbackResult">Result to return if not set.</param>
		public static TResult ForResult<TResult>(DialogContent content, TResult fallbackResult = default)
			=> (TResult)ExecuteOpen(DI.Get<DialogPageViewModel>(), content, null, fallbackResult);

		/// <summary>
		/// Opens a dialog with given content.
		/// </summary>
		/// <typeparam name="TPage">Dialog page to open.</typeparam>
		/// <typeparam name="TResult">Type of object the dialog will return.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="onClosed">Method to execute once the dialog closes.</param>
		/// <param name="fallbackResult">Result to return if not set.</param>
		public static TResult ForResult<TPage, TResult>(DialogContent content, Action<WindowViewModel> onClosed, TResult fallbackResult = default)
			where TPage : DialogPageViewModel
			=> (TResult)ExecuteOpen(DI.Get<TPage>(), content, options => options.OnClosed = onClosed, fallbackResult);

		/// <summary>
		/// Opens a dialog with given content on a page registered as <see cref="DialogPageViewModel"/>.
		/// </summary>
		/// <typeparam name="TResult">Type of object the dialog will return.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="onClosed">Method to execute once the dialog closes.</param>
		/// <param name="fallbackResult">Result to return if not set.</param>
		public static TResult ForResult<TResult>(DialogContent content, Action<WindowViewModel> onClosed, TResult fallbackResult = default)
			=> (TResult)ExecuteOpen(DI.Get<DialogPageViewModel>(), content, options => options.OnClosed = onClosed, fallbackResult);

		/// <summary>
		/// Opens a dialog with given content and window open options.
		/// </summary>
		/// <typeparam name="TPage">Dialog page to open.</typeparam>
		/// <typeparam name="TResult">Type of object the dialog will return.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="openOptionsSetup">Used for setting up window opening options.</param>
		/// <param name="fallbackResult">Result to return if not set.</param>
		public static TResult ForResult<TPage, TResult>(DialogContent content, Action<OpenWindowOptions> openOptionsSetup, TResult fallbackResult = default)
			where TPage : DialogPageViewModel
			=> (TResult)ExecuteOpen(DI.Get<TPage>(), content, openOptionsSetup, fallbackResult);

		/// <summary>
		/// Opens a dialog with given content and window open options.
		/// </summary>
		/// <typeparam name="TResult">Type of object the dialog will return.</typeparam>
		/// <param name="content">Content to show on the dialog.</param>
		/// <param name="openOptionsSetup">Used for setting up window opening options.</param>
		/// <param name="fallbackResult">Result to return if not set.</param>
		public static TResult ForResult<TResult>(DialogContent content, Action<OpenWindowOptions> openOptionsSetup, TResult fallbackResult = default)
			=> (TResult)ExecuteOpen(DI.Get<DialogPageViewModel>(), content, openOptionsSetup, fallbackResult);

		private static object ExecuteOpen(object pageViewModel, DialogContent content, Action<OpenWindowOptions> openOptionsSetup, object fallbackResult = null)
		{
			var openOptions = new OpenWindowOptions();

			openOptionsSetup?.Invoke(openOptions);

			openOptions.AdditionalOptions = content;
			openOptions.PageData = content;

			if (!openOptions.PreviousWindowAction.HasValue)
			{
				openOptions.PreviousWindowAction = WindowAction.LeaveShown;
			}

			if (!openOptions.Size.HasValue)
			{
				openOptions.Size = (600, 400);
			}
			
			_openMethod
				.MakeGenericMethod(_popupWindowViewModel.GetType(), pageViewModel.GetType())
				.Invoke(_windows, new object[] { openOptions });

			return content.Result ?? fallbackResult;
		}
	}
}
