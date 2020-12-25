using Braco.Utilities;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using Braco.Utilities.Wpf.Controls;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Braco.Generator
{
	[Page(AllowGoingToPreviousPage = true)]
	public class NewProjectPageViewModel : PageViewModel
	{
		private readonly MemberCheck[] _checks;
		private readonly IChooserDialogsService _dialogsService;
		private readonly ProjectManager _projectManager;

		#region Project Name

		private string _projectName;

		[Required]
		public string ProjectName
		{
			get => _projectName;
			set => _projectName = CorrectProjectName(_projectName, value);
		}

		#endregion

		public bool CreateNewWPFProject { get; set; }

		#region Existing Project Location

		private string _existingProjectLocation;
		public string ExistingProjectLocation
		{
			get => _existingProjectLocation;
			set
			{
				if (Equals(value, _existingProjectLocation)) return;

				_existingProjectLocation = PathUtilities.GetPathWithoutInvalidChars(value);
			}
		}

		#endregion

		#region New Project Name

		private string _newProjectName;
		public string NewProjectName
		{
			get => _newProjectName;
			set => _newProjectName = CorrectProjectName(_newProjectName, value);
		}

		#endregion

		#region New Project Location

		private string _newProjectLocation;
		public string NewProjectLocation
		{
			get => _newProjectLocation;
			set
			{
				if (Equals(value, _newProjectLocation)) return;

				_newProjectLocation = PathUtilities.GetPathWithoutInvalidChars(value);
			}
		}

		#endregion

		[Setting]
		public bool StartWPFProject { get; set; }

		[Setting]
		public bool StartWPFProjectWithVSCode { get; set; }

		public ICommand PickExistingProjectLocationCommand { get; }
		public ICommand PickNewProjectLocationCommand { get; }
		public ICommand CreateProjectCommand { get; }

		public NewProjectPageViewModel(IChooserDialogsService dialogsService, ProjectManager projectManager)
		{
			_dialogsService = dialogsService ?? throw new ArgumentNullException(nameof(dialogsService));
			_projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));

			PickExistingProjectLocationCommand = new RelayCommand(OnPickExistingProjectLocation);
			PickNewProjectLocationCommand = new RelayCommand(OnPickNewProjectLocation);
			CreateProjectCommand = new RelayCommand(OnCreateProject);

			_checks = new[]
			{
				new MemberCheck(() => CreateNewWPFProject ? true : ExistingProjectLocation.IsNotNullOrEmpty(), "Existing project location must be provided", nameof(ExistingProjectLocation)),
				new MemberCheck(() => CreateNewWPFProject ? true : ExistingProjectLocation.IsNullOrEmpty() || (File.Exists(ExistingProjectLocation) && ExistingProjectLocation.EndsWith(".csproj")), "Existing project location must be an existing .csproj file", nameof(ExistingProjectLocation)),
				new MemberCheck(() => CreateNewWPFProject ? NewProjectName.IsNotNullOrEmpty() : true, "New project name must be provided", nameof(NewProjectName)),
				new MemberCheck(() => CreateNewWPFProject ? NewProjectLocation.IsNotNullOrEmpty() : true, "New project location must be provided", nameof(NewProjectLocation)),
			};
		}

		private void OnPickExistingProjectLocation()
		{
			var location = _dialogsService.ChooseFile("Choose existing WPF project's location", ("C# Project", "*.csproj"));

			if (location == null) return;

			ExistingProjectLocation = location;
		}

		private void OnPickNewProjectLocation()
		{
			var location = _dialogsService.ChooseDirectory("Pick new WPF project's location");

			if (location == null) return;

			NewProjectLocation = location;
		}

		private void OnCreateProject()
		{
			if (!_validator.Validate(_checks)) return;

			var project = _projectManager.Find(ProjectName);

			if (project == null)
			{
				project = _projectManager.CreateProject(ProjectName);
			}
			else
			{
				var dialogContent = new DialogContent
				(
					title: "Override existing project?",
					content: $"The project named {ProjectName} already exists. Do you want to override it? {Environment.NewLine}If not, the existing one will be opened."
				)
				.AddResultButton(new ImageButtonViewModel("Confirm") { ButtonSize = "40" }, true)
				.AddResultButton(new ImageButtonViewModel("Cancel"), false);

				var dialogResult = Dialog.ForResult<bool?>(dialogContent);

				if (dialogResult == null) return;

				if (dialogResult == true)
				{
					project = _projectManager.CreateProject(ProjectName);
				}

				project.InitializeSections();
			}

			if (CreateNewWPFProject)
			{
				project.FileContent = new ProjectFileContent { CsprojPath = NewProjectLocation };
			}
			else
			{
				project.FileContent = new ProjectFileContent { CsprojPath = ExistingProjectLocation };
			}

			project.FileContent.SetDefaultTargetPaths();

			_projectManager.CurrentProject = project;
			ChangePage<ProjectHomePageViewModel>(project);
		}

		private string CorrectProjectName(string currentProjectName, string newProjectName)
		{
			if (Equals(newProjectName, currentProjectName)) return currentProjectName;

			if (PathUtilities.FileNameContainsInvalidChars(newProjectName) || PathUtilities.PathContainsInvalidChars(newProjectName))
			{
				currentProjectName = PathUtilities.GetFileNameWithoutInvalidChars(PathUtilities.GetPathWithoutInvalidChars(newProjectName));
				var @char = newProjectName.IsNullOrEmpty() ? '\0' : newProjectName.Last();
				ShowInfoBox(InfoBoxType.Warning, $"The project name cannot contain character '{@char}'!", 2);
			}
			else
			{
				currentProjectName = newProjectName;
			}

			return currentProjectName;
		}
	}
}
