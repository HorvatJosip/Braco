using Braco.Services.Abstractions;
using Braco.Utilities;
using Braco.Utilities.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Braco.Generator
{
	public class ProjectManager
	{
		public const string ProjectExtension = "braco";
		public const string ProjectsFilter = "*." + ProjectExtension;

		private readonly IFileManager _fileManager;
		private readonly IConfigurationService _configuration;

		private HashSet<FileViewModel> _blacklistedProjects;

		public IList<Project> Projects { get; private set; }

		public Project CurrentProject { get; set; }

		public string ProjectsDirectoryPath => _configuration[ConfigurationKeys.ProjectsDirectory];
		public DirectoryInfo ProjectsDirectory => new DirectoryInfo(ProjectsDirectoryPath);

		public ProjectManager(IFileManager fileManager, IConfigurationService configuration)
		{
			_fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

			var projectBlacklist = _configuration[ConfigurationKeys.ProjectBlacklist];

			_blacklistedProjects = projectBlacklist == null
				? new HashSet<FileViewModel>()
				: JsonConvert.DeserializeObject<HashSet<FileViewModel>>(projectBlacklist);
		}

		public Project CreateProject(string name)
		{
			if (Projects.IsNullOrEmpty()) LoadProjects();

			var projectsDirectoryPath = ProjectsDirectoryPath;

			var newProjectDirectoryPath = Path.Combine(projectsDirectoryPath, name);

			var newProjectDirectory = _fileManager.AddDirectory(newProjectDirectoryPath);

			newProjectDirectory.Delete(true);
			newProjectDirectory.Create();

			var projectFileName = $"{name}{PathUtilities.GetExtensionWithDot(ProjectExtension)}";

			var projectFile = new FileInfo(Path.Combine(newProjectDirectory.FullName, projectFileName));

			_fileManager.AddFileToDirectory(newProjectDirectory, projectFileName);

			var project = new Project(projectFile);

			project.InitializeSections();

			Projects.Add(project);

			return project;
		}

		public IEnumerable<FileViewModel> GetExistingProjects()
		{
			if (Projects.IsNullOrEmpty()) LoadProjects();

			return Projects
				.Select(project => FileViewModel.FromFile(project.File))
				.Where(project => !_blacklistedProjects.Contains(project));
		}

		public Project Find(FileViewModel existingProjectViewModel)
		{
			if (Projects.IsNullOrEmpty()) LoadProjects();

			return Projects.FirstOrDefault(project => project.File.FullName == existingProjectViewModel.Path);
		}

		public Project Find(string name)
		{
			if (Projects.IsNullOrEmpty()) LoadProjects();

			return Projects.FirstOrDefault(project => project.Name == name);
		}

		public void LoadProjects()
		{
			Projects = ProjectsDirectory.Exists
				? ProjectsDirectory
					.EnumerateFiles(ProjectsFilter, SearchOption.AllDirectories)
					.Select(projectFile => new Project(projectFile))
					.ToList()
				: new List<Project>();
		}

		public void ClearHistory()
		{
			_blacklistedProjects = Projects
				.Select(project => FileViewModel.FromFile(project.File))
				.ToHashSet();

			WriteBlacklistToConfiguration();
		}

		public bool AddToHistory(Project existingProject)
		{
			if (_blacklistedProjects.RemoveWhere(project => project.Path == existingProject.File.FullName) > 0)
			{
				WriteBlacklistToConfiguration();
				return true;
			}

			return false;
		}

		public bool RemoveFromHistory(FileViewModel existingProject)
		{
			if (_blacklistedProjects.Add(existingProject))
			{
				WriteBlacklistToConfiguration();
				return true;
			}

			return false;
		}

		private void WriteBlacklistToConfiguration()
		{
			_configuration.Set(ConfigurationKeys.ProjectBlacklist, JsonConvert.SerializeObject(_blacklistedProjects));
		}
	}
}
