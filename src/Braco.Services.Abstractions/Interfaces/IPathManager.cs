using System.IO;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Used for managing paths.
	/// </summary>
	public interface IPathManager
	{
		/// <summary>
		/// Directory key for the application directory.
		/// </summary>
		const string AppDirectoryKey = "AppDir";

		/// <summary>
		/// Directory key for the AppData directory.
		/// </summary>
		const string AppDataDirectoryKey = "AppDataDir";

		/// <summary>
		/// <see cref="DirectoryInfo"/> about application directory.
		/// </summary>
		DirectoryInfo AppDirectory { get; }

		/// <summary>
		/// <see cref="DirectoryInfo"/> about AppData directory.
		/// </summary>
		DirectoryInfo AppDataDirectory { get; }

		/// <summary>
		/// Gets info about a file by key.
		/// </summary>
		/// <param name="key">Key used for the file.</param>
		/// <returns><see cref="FileInfo"/> about desired file.</returns>
		FileInfo GetFile(string key);

		/// <summary>
		/// Gets info about a directory by key.
		/// </summary>
		/// <param name="key">Key used for the directory.</param>
		/// <returns><see cref="DirectoryInfo"/> about desired directory.</returns>
		DirectoryInfo GetDirectory(string key);

		/// <summary>
		/// Adds file to the directory.
		/// </summary>
		/// <param name="key">Key used for the file.</param>
		/// <param name="directory">Directory in which to place the file.</param>
		/// <param name="fileName">Name of the file to add.</param>
		/// <param name="removeInvalidChars">Should the invalid file name characters be removed?</param>
		/// <returns><see cref="FileInfo"/> about added file or null if it wasn't added.</returns>
		FileInfo AddFileToDirectory(string key, DirectoryInfo directory, string fileName, bool removeInvalidChars = true);

		/// <summary>
		/// Adds a file to the collection.
		/// </summary>
		/// <param name="key">Key used to identify the file.</param>
		/// <param name="path">Path of the file.</param>
		/// <param name="removeInvalidChars">Should the invalid characters be removed from the file name?</param>
		/// <returns><see cref="FileInfo"/> about added file or null if it wasn't added.</returns>
		FileInfo AddFile(string key, string path, bool removeInvalidChars = true);

		/// <summary>
		/// Adds a directory to the collection.
		/// </summary>
		/// <param name="key">Key used to identify the directory.</param>
		/// <param name="path">Path of the directory.</param>
		/// <param name="removeInvalidChars">Should the invalid characters be removed from the directory path?</param>
		/// <returns><see cref="DirectoryInfo"/> about added directory or null if it wasn't added.</returns>
		DirectoryInfo AddDirectory(string key, string path, bool removeInvalidChars = true);
	}
}
