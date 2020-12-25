using System;
using System.Windows.Input;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Represents an image button.
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class ImageButtonViewModel
	{
		/// <summary>
		/// Name of the icon file.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Name of the icon getter.
		/// </summary>
		public string FileNameGetter { get; set; }

		/// <summary>
		/// Command to fire on click.
		/// </summary>
		public ICommand Command { get; set; }

		/// <summary>
		/// Is the image button enabled?
		/// </summary>
		public bool IsEnabled { get; set; } = true;

		/// <summary>
		/// Tooltip to show on hover.
		/// </summary>
		public string ToolTip { get; set; }

		/// <summary>
		/// Size of the image button.
		/// </summary>
		public string ButtonSize { get; set; } = "20";

		/// <summary>
		/// Creates a default instance of the image button.
		/// </summary>
		public ImageButtonViewModel() { }

		/// <summary>
		/// Creates an instance of the image button with the given action to execute on click.
		/// </summary>
		/// <param name="commandAction">Action to fire on click.</param>
		public ImageButtonViewModel(Action commandAction) : this(new RelayCommand(commandAction)) { }

		/// <summary>
		/// Creates an instance of the image button with the given command to fire on click.
		/// </summary>
		/// <param name="command">Command to fire on click.</param>
		public ImageButtonViewModel(ICommand command)
		{
			Command = command;
		}

		/// <summary>
		/// Creates an instance of the image button with the file name of the icon to use.
		/// </summary>
		/// <param name="fileName">Name of the icon file.</param>
		public ImageButtonViewModel(string fileName)
		{
			FileName = fileName;
		}

		/// <summary>
		/// Creates an instance of the image button with the given command to fire on click
		/// and a file name of the icon to use.
		/// </summary>
		/// <param name="fileName">Name of the icon file.</param>
		/// <param name="command">Command to fire on click.</param>
		public ImageButtonViewModel(string fileName, ICommand command) : this(command)
		{
			FileName = fileName;
		}

		/// <summary>
		/// Creates an instance of the image button with the given action to execute on click
		/// and a file name of the icon to use.
		/// </summary>
		/// <param name="fileName">Name of the icon file.</param>
		/// <param name="commandAction">Action to execute on click.</param>
		public ImageButtonViewModel(string fileName, Action commandAction) : this(commandAction)
		{
			FileName = fileName;
		}
	}
}
