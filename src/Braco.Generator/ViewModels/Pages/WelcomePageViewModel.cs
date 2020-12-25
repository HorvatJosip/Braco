using Braco.Utilities;
using Braco.Utilities.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Braco.Generator
{
	public class WelcomePageViewModel : PageViewModel
	{
		private readonly IChooserDialogsService _dialogs;
		private readonly ProjectManager _projectManager;

		public ObservableCollection<FileViewModel> ExistingProjects { get; set; }

		public ICommand CreateNewProjectCommand { get; }
		public ICommand OpenExistingProjectCommand { get; }
		public ICommand RemoveExistingProjectCommand { get; }
		public ICommand OpenExistingProjectFromFileCommand { get; }
		public ICommand ClearHistoryCommand { get; }

		public WelcomePageViewModel(IChooserDialogsService dialogs, ProjectManager projectManager)
		{
			_dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
			_projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));

			ExistingProjects = new ObservableCollection<FileViewModel>(_projectManager.GetExistingProjects());

			CreateNewProjectCommand = new RelayCommand(OnCreateNewProject);
			OpenExistingProjectCommand = new RelayCommand<FileViewModel>(OnOpenExistingProject);
			RemoveExistingProjectCommand = new RelayCommand<FileViewModel>(OnRemoveExistingProject);
			OpenExistingProjectFromFileCommand = new RelayCommand(OnOpenExistingProjectFromFile);
			ClearHistoryCommand = new RelayCommand(OnClearHistory);
		}

		private void OnCreateNewProject()
		{
			ChangePage<NewProjectPageViewModel>();
		}

		private void OnOpenExistingProject(FileViewModel project)
		{
			var existingProject = _projectManager.Find(project);

			if (existingProject == null)
			{
				ShowErrorInInfoBox($"The project {project.Name} wasn't found...");
				return;
			}

			_projectManager.AddToHistory(existingProject);

			Open(existingProject);
		}

		private void Open(Project project)
		{
			project.InitializeSections();
			_projectManager.CurrentProject = project;
			ChangePage<ProjectHomePageViewModel>(project);
		}

		private void OnOpenExistingProjectFromFile()
		{
			var projectPath = _dialogs.ChooseFile("Open an existing project", null, ("Project file", ProjectManager.ProjectsFilter));

			if (projectPath == null) return;

			OnOpenExistingProject(FileViewModel.FromPath(projectPath));
		}

		private void OnRemoveExistingProject(FileViewModel existingProject)
		{
			_projectManager.RemoveFromHistory(existingProject);
			ExistingProjects.Remove(existingProject);
		}

		private void OnClearHistory()
		{
			// TODO: yes-no dialog?
			_projectManager.ClearHistory();
			ExistingProjects.Clear();
		}
	}
}
