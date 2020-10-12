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
		/// <param name="terminateAfter">Should the process terminate after execution finishes?</param>
		/// <param name="useShellExecute">Should the system shell be used to start the process?</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommand(string command, DirectoryInfo workingDirectory, bool terminateAfter, bool useShellExecute);

		/// <summary>
		/// Used for executing a command.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="workingDirectory">Working directory in which to execute the command.</param>
		/// <param name="terminateAfter">Should the process terminate after execution finishes?</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommand(string command, DirectoryInfo workingDirectory, bool terminateAfter);

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
		/// <param name="terminateAfter">Should the process terminate after execution finishes?</param>
		/// <param name="useShellExecute">Should the system shell be used to start the process?</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommand(string command, bool terminateAfter, bool useShellExecute);

		/// <summary>
		/// Used for executing a command. in the current directory
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="terminateAfter">Should the process terminate after execution finishes?</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommand(string command, bool terminateAfter);

		/// <summary>
		/// Used for executing a command in the current directory.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommand(string command);

		/// <summary>
		/// Used for opening a directory as admin.
		/// </summary>
		/// <param name="directory">Directory to open.</param>
		/// <returns>Process that was started by opening the directory.</returns>
        Process OpenDirectoryAsAdmin(DirectoryInfo directory);

		/// <summary>
		/// Used for opening a file as admin.
		/// </summary>
		/// <param name="file">File to open.</param>
		/// <returns>Process that was started by opening the file.</returns>
		Process OpenFileAsAdmin(FileInfo file);

		/// <summary>
		/// Used for executing a command as admin.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="workingDirectory">Working directory in which to execute the command.</param>
		/// <param name="terminateAfter">Should the process terminate after execution finishes?</param>
		/// <param name="useShellExecute">Should the system shell be used to start the process?</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommandAsAdmin(string command, DirectoryInfo workingDirectory, bool terminateAfter, bool useShellExecute);

		/// <summary>
		/// Used for executing a command as admin.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="workingDirectory">Working directory in which to execute the command.</param>
		/// <param name="terminateAfter">Should the process terminate after execution finishes?</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommandAsAdmin(string command, DirectoryInfo workingDirectory, bool terminateAfter);

		/// <summary>
		/// Used for executing a command as admin.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="workingDirectory">Working directory in which to execute the command.</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommandAsAdmin(string command, DirectoryInfo workingDirectory);

		/// <summary>
		/// Used for executing a command in the current directory as admin.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="terminateAfter">Should the process terminate after execution finishes?</param>
		/// <param name="useShellExecute">Should the system shell be used to start the process?</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommandAsAdmin(string command, bool terminateAfter, bool useShellExecute);

		/// <summary>
		/// Used for executing a command. in the current directory as admin.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="terminateAfter">Should the process terminate after execution finishes?</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommandAsAdmin(string command, bool terminateAfter);

		/// <summary>
		/// Used for executing a command in the current directory as admin.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <returns>Process that was started by running the command.</returns>
		Process ExecuteCommandAsAdmin(string command);

		/// <summary>
		/// Used for starting a process.
		/// </summary>
		/// <param name="startInfo">Options for starting the process.</param>
		/// <returns>The started process.</returns>
		Process Start(ProcessStartInfo startInfo);
    }
}
