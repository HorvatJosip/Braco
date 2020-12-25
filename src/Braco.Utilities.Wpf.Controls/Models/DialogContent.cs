using Braco.Services;
using System;
using System.Collections.Generic;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Content for a dialog.
	/// </summary>
	public class DialogContent
	{
		/// <summary>
		/// Title for the dialog.
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// Content of the dialog.
		/// </summary>
		public string Content { get; set; }
		/// <summary>
		/// Buttons to show on the dialog.
		/// </summary>
		public List<ImageButtonViewModel> Buttons { get; set; } = new List<ImageButtonViewModel>();

		/// <summary>
		/// Result for when the dialog closes.
		/// </summary>
		public object Result { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DialogContent() { }

		/// <summary>
		/// Initializes the dialog content with a title.
		/// </summary>
		/// <param name="title">Title for the dialog.</param>
		public DialogContent(string title)
		{
			Title = title;
		}

		/// <summary>
		/// Initializes the dialog content with a title and content.
		/// </summary>
		/// <param name="title">Title for the dialog.</param>
		/// <param name="content">Content of the dialog.</param>
		public DialogContent(string title, string content) : this(title)
		{
			Content = content;
		}

		/// <summary>
		/// Initializes the dialog content with a title, content and buttons.
		/// </summary>
		/// <param name="title">Title for the dialog.</param>
		/// <param name="content">Content of the dialog.</param>
		/// <param name="buttons">Buttons to show on the dialog.</param>
		public DialogContent(string title, string content, params ImageButtonViewModel[] buttons)
			: this(title, content)
		{
			Buttons = new List<ImageButtonViewModel>(buttons);
		}

		/// <summary>
		/// Used for adding a button that will close the window and set the <see cref="Result"/>.
		/// </summary>
		/// <param name="button">Data for the button.</param>
		/// <param name="result">Result for when the dialog closes.</param>
		/// <returns>Current instance of dialog content</returns>
		public DialogContent AddResultButton(ImageButtonViewModel button, object result)
		{
			if (button == null) throw new ArgumentNullException(nameof(button));

			button.Command = new RelayCommand(() =>
			{
				Result = result;
				DI.Get<IWindowsManager>().ActiveWindow.Close();
			});

			Buttons.Add(button);

			return this;
		}
	}
}
