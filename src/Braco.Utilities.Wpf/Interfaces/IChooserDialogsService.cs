using System.Collections.Generic;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Service used for working with dialogs that allow
    /// the user to choose a file or directory location.
    /// </summary>
    public interface IChooserDialogsService
	{
		/// <summary>
		/// Allows the user to choose a file.
		/// </summary>
		/// <param name="title">Title displayed on file chooser dialog.</param>
		/// <param name="filters">Extensions for files that can be picked.</param>
		/// <returns>Picked file path or null if nothing was picked.</returns>
		string ChooseFile(string title, params (string label, string extensions)[] filters);

		/// <summary>
		/// Allows the user to choose a file.
		/// </summary>
		/// <param name="title">Title displayed on file chooser dialog.</param>
		/// <param name="initialDirectory">Directory in which the dialog should start.</param>
		/// <param name="filters">Extensions for files that can be picked.</param>
		/// <returns>Picked file path or null if nothing was picked.</returns>
		string ChooseFile(string title, string initialDirectory, params (string label, string extensions)[] filters);
		
		/// <summary>
		/// Allows the user to choose a file.
		/// </summary>
		/// <param name="title">Title displayed on file chooser dialog.</param>
		/// <param name="initialDirectory">Directory in which the dialog should start.</param>
		/// <param name="selectedFile">File that is selected in advance.</param>
		/// <param name="filters">Extensions for files that can be picked.</param>
		/// <returns>Picked file path or null if nothing was picked.</returns>
		string ChooseFile(string title, string initialDirectory, string selectedFile, params (string label, string extensions)[] filters);

		/// <summary>
		/// Allows the user to choose multiple files.
		/// </summary>
		/// <param name="title">Title displayed on file chooser dialog.</param>
		/// <param name="filters">Extensions for files that can be picked.</param>
		/// <returns>Picked file paths or null if nothing was picked.</returns>
		IList<string> ChooseFiles(string title, params (string label, string extensions)[] filters);

		/// <summary>
		/// Allows the user to choose multiple files.
		/// </summary>
		/// <param name="title">Title displayed on file chooser dialog.</param>
		/// <param name="initialDirectory">Directory in which the dialog should start.</param>
		/// <param name="filters">Extensions for files that can be picked.</param>
		/// <returns>Picked file paths or null if nothing was picked.</returns>
		IList<string> ChooseFiles(string title, string initialDirectory, params (string label, string extensions)[] filters);
		
		/// <summary>
		/// Allows the user to choose multiple files.
		/// </summary>
		/// <param name="title">Title displayed on file chooser dialog.</param>
		/// <param name="initialDirectory">Directory in which the dialog should start.</param>
		/// <param name="selectedFile">File that is selected in advance.</param>
		/// <param name="filters">Extensions for files that can be picked.</param>
		/// <returns>Picked file paths or null if nothing was picked.</returns>
		IList<string> ChooseFiles(string title, string initialDirectory, string selectedFile, params (string label, string extensions)[] filters);

		/// <summary>
		/// Allows the user to choose a directory.
		/// </summary>
		/// <param name="title">Title of the directory chooser dialog.</param>
		/// <param name="initialDirectory">Directory that is selected initially.</param>
		/// <returns>Picked directory path or null if nothing was picked.</returns>
		string ChooseDirectory(string title, string initialDirectory = null);
    }
}