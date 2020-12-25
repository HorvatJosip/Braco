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
		/// <summary>
		/// Name of the explorer executable file.
		/// </summary>
		public const string ExplorerFileName = "explorer.exe";
		/// <summary>
		/// Name of the command prompt executable file.
		/// </summary>
        public const string CmdFileName = "cmd.exe";

		/// <summary>
		/// Parameter that specifies cmd to carry out 
		/// the command specified by string and then terminate.
		/// <para>To check for other parameters, open a command prompt and run "cmd /?"</para>
		/// </summary>
		public const string TerminateFlag = "/C";

		/// <summary>
		/// To start a process as admin, specify this as <see cref="ProcessStartInfo.Verb"/>.
		/// </summary>
		public const string AdminVerb = "runas";

		private readonly IFileManager _fileManager;
		private readonly bool _defaultTerminateAfter;
		private readonly bool _defaultUseShellExecute;

		/// <summary>
		/// Creates an instance of the manager.
		/// </summary>
		/// <param name="fileManager">File manager for the project.</param>
		/// <param name="defaultTerminateAfter">Default value that will be used for "terminateAfter"
		/// argument if not specified in the argument list.</param>
		/// <param name="defaultUseShellExecute">Default value that will be used for "useShellExecute"
		/// argument if not specified in the argument list.</param>
		public WindowsProcessStarter(IFileManager fileManager, bool defaultTerminateAfter, bool defaultUseShellExecute)
		{
			_fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
			_defaultTerminateAfter = defaultTerminateAfter;
			_defaultUseShellExecute = defaultUseShellExecute;
		}

		/// <inheritdoc/>
		public Process OpenDirectory(DirectoryInfo directory)
			=> OpenDirectory(directory, runAsAdmin: false);

		/// <inheritdoc/>
        public Process OpenFile(FileInfo file)
			=> OpenFile(file, runAsAdmin: false);

		/// <inheritdoc/>
		public Process ExecuteCommand(string command, DirectoryInfo workingDirectory, bool terminateAfter, bool useShellExecute)
			=> ExecuteCommand(command, workingDirectory, terminateAfter, useShellExecute, runAsAdmin: false);

		/// <inheritdoc/>
		public Process ExecuteCommand(string command, DirectoryInfo workingDirectory, bool terminateAfter)
			=> ExecuteCommand(command, workingDirectory, terminateAfter, useShellExecute: _defaultUseShellExecute);

		/// <inheritdoc/>
		public Process ExecuteCommand(string command, DirectoryInfo workingDirectory)
			=> ExecuteCommand(command, workingDirectory, terminateAfter: _defaultTerminateAfter);

		/// <inheritdoc/>
		public Process ExecuteCommand(string command, bool terminateAfter, bool useShellExecute)
			=> ExecuteCommand(command, workingDirectory: null, terminateAfter, useShellExecute);

		/// <inheritdoc/>
		public Process ExecuteCommand(string command, bool terminateAfter)
			=> ExecuteCommand(command, terminateAfter, useShellExecute: _defaultUseShellExecute);

		/// <inheritdoc/>
		public Process ExecuteCommand(string command)
			=> ExecuteCommand(command, terminateAfter: _defaultTerminateAfter);

		/// <inheritdoc/>
		public Process OpenDirectoryAsAdmin(DirectoryInfo directory)
			=> OpenDirectory(directory, runAsAdmin: true);

		/// <inheritdoc/>
		public Process OpenFileAsAdmin(FileInfo file)
			=> OpenFile(file, runAsAdmin: true);

		/// <inheritdoc/>
		public Process ExecuteCommandAsAdmin(string command, DirectoryInfo workingDirectory, bool terminateAfter, bool useShellExecute)
			=> ExecuteCommand(command, workingDirectory, terminateAfter, useShellExecute, runAsAdmin: true);

		/// <inheritdoc/>
		public Process ExecuteCommandAsAdmin(string command, DirectoryInfo workingDirectory, bool terminateAfter)
			=> ExecuteCommandAsAdmin(command, workingDirectory, terminateAfter, useShellExecute: _defaultUseShellExecute);

		/// <inheritdoc/>
		public Process ExecuteCommandAsAdmin(string command, DirectoryInfo workingDirectory)
			=> ExecuteCommandAsAdmin(command, workingDirectory, terminateAfter: _defaultTerminateAfter);

		/// <inheritdoc/>
		public Process ExecuteCommandAsAdmin(string command, bool terminateAfter, bool useShellExecute)
			=> ExecuteCommandAsAdmin(command, workingDirectory: null, terminateAfter, useShellExecute);

		/// <inheritdoc/>
		public Process ExecuteCommandAsAdmin(string command, bool terminateAfter)
			=> ExecuteCommandAsAdmin(command, terminateAfter, useShellExecute: _defaultUseShellExecute);

		/// <inheritdoc/>
		public Process ExecuteCommandAsAdmin(string command)
			=> ExecuteCommandAsAdmin(command, terminateAfter: _defaultTerminateAfter);

		private Process OpenFile(FileInfo file, bool runAsAdmin)
		{
			if (file == null) throw new ArgumentNullException(nameof(file));

			return Start(Generate
			(
				fileName: file.FullName,
				arguments: string.Empty,
				workingDirectory: null,
				useShellExecute: true,
				runAsAdmin: runAsAdmin
			));
		}

		private Process OpenDirectory(DirectoryInfo directory, bool runAsAdmin)
		{
			if (directory == null) throw new ArgumentNullException(nameof(directory));

			return Start(Generate
			(
				fileName: ExplorerFileName,
				arguments: $"\"{directory.FullName}\"",
				workingDirectory: null,
				useShellExecute: true,
				runAsAdmin: runAsAdmin
			));
		}

		private Process ExecuteCommand(string command, DirectoryInfo workingDirectory, bool terminateAfter, bool useShellExecute, bool runAsAdmin)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			return Start(Generate
			(
				fileName: CmdFileName,
				arguments: $"{(terminateAfter ? $"{TerminateFlag} " : "")}{command}",
				workingDirectory: workingDirectory,
				useShellExecute: useShellExecute,
				runAsAdmin: runAsAdmin
			));
		}

		/// <summary>
		/// Used for quickly generating <see cref="ProcessStartInfo"/>.
		/// </summary>
		/// <param name="fileName">Name of the file to run.</param>
		/// <param name="arguments">Arguments for running the file.</param>
		/// <param name="workingDirectory">Directory where to position before running.</param>
		/// <param name="useShellExecute">Should the system shell be used to start the process?</param>
		/// <param name="runAsAdmin">Should the process be started as administrator?</param>
		/// <returns>Instance of the configured <see cref="ProcessStartInfo"/>.</returns>
		public ProcessStartInfo Generate(string fileName, string arguments, DirectoryInfo workingDirectory, bool useShellExecute, bool runAsAdmin)
		{
			var instance = new ProcessStartInfo
			{
				FileName = fileName,
				Arguments = arguments,
				WorkingDirectory = workingDirectory?.FullName ?? _fileManager.AppDirectory.FullName,
				UseShellExecute = useShellExecute,
				ErrorDialog = true
			};

			if (runAsAdmin)
			{
				instance.Verb = AdminVerb;
			}

			return instance;
		}

		/// <inheritdoc/>
		public Process Start(ProcessStartInfo startInfo)
			=> Process.Start(startInfo);
	}
}
