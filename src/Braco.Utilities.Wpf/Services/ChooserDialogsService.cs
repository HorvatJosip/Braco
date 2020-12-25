using Braco.Utilities.Extensions;
using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Implementation of <see cref="IChooserDialogsService"/> using
	/// <see cref="Microsoft.WindowsAPICodePack.Dialogs"/>.
	/// </summary>
    public class ChooserDialogsService : IChooserDialogsService
    {
		/// <inheritdoc/>
        public string ChooseDirectory(string title, string initialDirectory)
        {
            using var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = title,
                Multiselect = false,
                InitialDirectory = initialDirectory,
            };

            // If the user chose something...
            return dialog.ShowDialog() == CommonFileDialogResult.Ok
                // Return the selected directory
                ? dialog.FileName
                // Otherwise, just return null
                : null;
        }

		/// <inheritdoc/>
		public string ChooseFile(string title, params (string label, string extensions)[] filters)
			=> Choose(title, null, null, false, filters)?.ElementAt(0);

		/// <inheritdoc/>
		public string ChooseFile(string title, string initialDirectory, params (string label, string extensions)[] filters)
			=> Choose(title, initialDirectory, null, false, filters)?.ElementAt(0);

		/// <inheritdoc/>
		public string ChooseFile(string title, string initialDirectory, string selectedFile, params (string label, string extensions)[] filters)
			=> Choose(title, initialDirectory, selectedFile, false, filters)?.ElementAt(0);

		/// <inheritdoc/>
		public IList<string> ChooseFiles(string title, params (string label, string extensions)[] filters)
			=> Choose(title, null, null, true, filters)?.ToList();

		/// <inheritdoc/>
		public IList<string> ChooseFiles(string title, string initialDirectory, params (string label, string extensions)[] filters)
			=> Choose(title, initialDirectory, null, true, filters)?.ToList();

		/// <inheritdoc/>
		public IList<string> ChooseFiles(string title, string initialDirectory, string selectedFile, params (string label, string extensions)[] filters)
			=> Choose(title, initialDirectory, selectedFile, true, filters)?.ToList();

		private IEnumerable<string> Choose(string title, string initialDirectory, string selectedFile, bool allowMultiple, params (string label, string extensions)[] filters)
		{
			// Create an open file dialog
			using var dialog = new CommonOpenFileDialog
			{
				EnsureFileExists = true,
				EnsurePathExists = true,
				Multiselect = allowMultiple,
				DefaultFileName = selectedFile,
				InitialDirectory = initialDirectory,
				Title = title
			};

			// The extensionList can use a semicolon(";") or comma (",") to separate extensions.
			// Extensions can be prefaced with a period (".") or with the file wild card specifier "*.".
			filters.ForEach(filter => dialog.Filters.Add(new CommonFileDialogFilter(
				rawDisplayName: filter.label,
				extensionList: filter.extensions
			)));

			// If the user chose something...
			return dialog.ShowDialog() == CommonFileDialogResult.Ok
				// Return the selected file(s)
				? dialog.FileNames
				// Otherwise, just return null
				: null;
		}
	}
}