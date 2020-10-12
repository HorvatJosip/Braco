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

        private IEnumerable<string> Choose(string title, string selectedFile, bool allowMultiple, params (string label, string extensions)[] filters)
        {
            // Create an open file dialog
            using var dialog = new CommonOpenFileDialog
            {
                EnsureFileExists = true,
                EnsurePathExists = true,
                Multiselect = allowMultiple,
                DefaultFileName = selectedFile,
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

		/// <inheritdoc/>
        public IList<string> ChooseFiles(string title, string selectedFile = null, params (string label, string extensions)[] filters)
            => Choose(title, selectedFile, true, filters)?.ToList();

		/// <inheritdoc/>
        public string ChooseFile(string title, string selectedFile, params (string label, string extensions)[] filters)
            => Choose(title, selectedFile, false, filters)?.ElementAt(0);
    }
}