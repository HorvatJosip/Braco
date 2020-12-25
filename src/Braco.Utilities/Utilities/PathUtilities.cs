using Braco.Utilities.Extensions;
using System.IO;
using System.Linq;

namespace Braco.Utilities
{
	/// <summary>
	/// Utilities for paths.
	/// </summary>
    public static class PathUtilities
    {
        #region Fields

        private static readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
        private static readonly char[] invalidPathChars = Path.GetInvalidPathChars();

        #endregion

        #region Methods

		/// <summary>
		/// Gets file name without invalid characters in it.
		/// </summary>
		/// <param name="fileName">File name to clean.</param>
		/// <returns>File name without invalid characters.</returns>
        public static string GetFileNameWithoutInvalidChars(string fileName)
            => new string(fileName.Where(c => !invalidFileNameChars.Contains(c)).ToArray());

		/// <summary>
		/// Gets path without invalid characters in it.
		/// </summary>
		/// <param name="path">Path to clean.</param>
		/// <returns>Path without invalid characters.</returns>
		public static string GetPathWithoutInvalidChars(string path)
            => new string(path.Where(c => !invalidPathChars.Contains(c)).ToArray());

		/// <summary>
		/// Checks if the given file name has invalid characters.
		/// </summary>
		/// <param name="fileName">File name to check.</param>
		/// <returns>If the given file name has invalid characters.</returns>
        public static bool FileNameContainsInvalidChars(string fileName)
            => fileName.Any(c => invalidFileNameChars.Contains(c));

		/// <summary>
		/// Checks if the given path has invalid characters.
		/// </summary>
		/// <param name="path">Path to check.</param>
		/// <returns>If the given path has invalid characters.</returns>
		public static bool PathContainsInvalidChars(string path)
            => path.Any(c => invalidPathChars.Contains(c));

		/// <summary>
		/// Gets the extension with the dot included.
		/// </summary>
		/// <param name="extension">Extension to get the dot with.</param>
		/// <returns>Extension with the dot at the start. Or null if extension is null</returns>
        public static string GetExtensionWithDot(string extension)
        {
            if (extension == null) return null;

            return extension.StartsWith('.') ? extension : $".{extension}";
        }

		/// <summary>
		/// Checks if all of the given extensions are equal.
		/// <para>Note: if no extensions or only one extension is provided, false is returned.</para>
		/// </summary>
		/// <param name="extensionsToTest">Collection of extensions to test if they are all equal.</param>
		/// <returns>If all of the given extensions are equal or not.</returns>
		public static bool AreExtensionsEqual(params string[] extensionsToTest)
		{
			if (extensionsToTest.IsNullOrEmpty() || extensionsToTest.Length == 1) return false;

			return extensionsToTest
				.Select(extension => GetExtensionWithDot(extension))
				.ToHashSet()
				.Count == 1;
		}

        #endregion
    }
}
