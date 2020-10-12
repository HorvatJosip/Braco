using System.Diagnostics;
using System.IO;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Defines process starting on an OS.
	/// </summary>
    public interface IProcessStarter
    {
		/// <summary>
		/// Used for opening a directory.
		/// </summary>
		/// <param name="directory">Directory to open.</param>
		/// <returns>Process that was started by opening the directory.</returns>
        Process OpenDirectory(DirectoryInfo directory);

		/// <summary>
		/// Used for opening a file.
		/// </summary>
		/// <param name="file">File to open.</param>
		/// <returns>Process that was started by opening the file.</returns>
		Process OpenFile(FileInfo file);

		/// <summary>
		/// Used for executing a command.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="workingDirectory">Working directory in which to execute the command.</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommand(string command, DirectoryInfo workingDirectory);

		/// <summary>
		/// Used for executing a command in the current directory.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommand(string command);
    }
}
