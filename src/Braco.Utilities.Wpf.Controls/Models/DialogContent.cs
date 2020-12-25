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
		public string Title { get; set; }
		public string Content { get; set; }
		public List<ImageButtonViewModel> Buttons { get; set; } = new List<ImageButtonViewModel>();
		public object Result { get; set; }

		public DialogContent() { }

		public DialogContent(string title)
		{
			Title = title;
		}

		public DialogContent(string title, string content) : this(title)
		{
			Content = content;
		}

		public DialogContent(string title, string content, params ImageButtonViewModel[] buttons)
			: this(title, content)
		{
			Buttons = new List<ImageButtonViewModel>(buttons);
		}

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
