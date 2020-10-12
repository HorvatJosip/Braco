using Braco.Services.Abstractions;
using System;
using System.Diagnostics;
using System.IO;

namespace Braco.Services
{
	/// <summary>
	/// Implementation of <see cref="IProcessStarter"/> for Windows OS.
	/// </summary>
    public class WindowsProcessStarter : IProcessStarter
    {
		private const string explorerFileName = "explorer.exe";
        private const string cmdFileName = "cmd.exe";
        private const string commandFormat = "/C {0}";
		private readonly IPathManager _pathManager;

		/// <summary>
		/// Creates an instance of the manager.
		/// </summary>
		/// <param name="pathManager">Path manager for the project.</param>
		public WindowsProcessStarter(IPathManager pathManager)
		{
			_pathManager = pathManager;
		}

		/// <inheritdoc/>
        public Process OpenDirectory(DirectoryInfo directory)
        {
            if (directory == null) throw new ArgumentNullException(nameof(directory));
            if (!directory.Exists) throw new ArgumentException("Cannot open a directory that doesn't exist.", nameof(directory));

            return Process.Start(new ProcessStartInfo(explorerFileName, directory.FullName)
            {
                UseShellExecute = true
            });
        }

		/// <inheritdoc/>
        public Process OpenFile(FileInfo file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (!file.Exists) throw new ArgumentException("Cannot open a file that doesn't exist.", nameof(file));

            return Process.Start(new ProcessStartInfo(file.FullName)
            {
                UseShellExecute = true
            });
        }

		/// <inheritdoc/>
        public Process ExecuteCommand(string command, DirectoryInfo workingDirectory)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (workingDirectory == null) throw new ArgumentNullException(nameof(workingDirectory));

            return Process.Start(new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory.FullName,
                Arguments = string.Format(commandFormat, command),
                FileName = cmdFileName,
            });
        }

		/// <inheritdoc/>
        public Process ExecuteCommand(string command)
            => ExecuteCommand(command, _pathManager.AppDirectory);
    }
}
