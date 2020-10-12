using Braco.Services;
using Braco.Utilities.Extensions;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting a page type into a <see cref="Frame"/> source.
	/// </summary>
	public class PageTypeToFrameSourceConverter : BaseConverter<PageTypeToFrameSourceConverter>
    {
        /// <summary>
        /// Folder in which the pages should reside.
        /// </summary>
        public const string PagesFolder = "Pages";
        /// <summary>
        /// Suffix for all of the created pages.
        /// </summary>
        public const string PageSuffix = "Page";
		/// <summary>
		/// Separator used to specify subfolders in the project.
		/// </summary>
		public const string FolderSeparator = "/";

		/// <summary>
		/// Name of the assembly for full pack uri.
		/// If left null, <see cref="AssemblyGetter"/> will be used.
		/// </summary>
		public string AssemblyName { get; set; }

		/// <summary>
		/// Format used for constructing a <see cref="Uri"/> to appropriate page.
		/// </summary>
		public string PageLocationFormat { get; set; } = $"{{0}}{FolderSeparator}{{1}}.xaml";

		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			// If we got a page type...
			if(value is Type page)
			{
				// Get info about the page
				var attr = page.GetCustomAttribute<PageAttribute>();

				// Get the assembly name (make sure we have an assembly from which to construct the uri)
				var assemblyName = AssemblyName ?? DI.Resources?.Get<AssemblyGetter, string>() ?? throw new InvalidOperationException($"Assembly name is required. Either set {nameof(AssemblyName)} or register {nameof(AssemblyGetter)} implementation.");

				// Get the page name
				var pageName = attr?.Name;

				// If it wasn't specified...
				if (pageName.IsNullOrEmpty())
				{
					// Get the name from view model's type
					pageName = page.Name
						// Remove the view model suffix
						.Replace(ContentViewModel.ViewModelSuffix, "");

					// If we don't have a page suffix...
					if (!pageName.EndsWith(PageSuffix))
						// Add it
						pageName += PageSuffix;
				}

				// Declare subfolders
				var subfolders = "";

				// If there are some defined...
				if(attr?.Subfolders?.Count() > 0)
				{
					// Construct them for the uri
					subfolders = string.Join(FolderSeparator, attr.Subfolders) + FolderSeparator;
				}

				// Create a page uri from gathered data
				return new Uri(PackUtilities.GetRootPackUriWithSuffix
				(
					assemblyName: assemblyName,
					suffix: string.Format
					(
						format: PageLocationFormat,
						arg0: PagesFolder,
						arg1: $"{subfolders}{pageName}"
					)
				));
			}

            // Otherwise, return null
            return null;
        }

		/// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
