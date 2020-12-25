using Braco.Utilities;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Braco.Generator
{
	[Page("Tabs", AllowGoingToPreviousPage = true)]
	public class PagesPageViewModel : PageViewModel
	{
		private const string frontendSuffix = "Page.xaml";
		private const string backendSuffix = "Page.xaml.cs";
		private const string viewModelSuffix = "PageViewModel.cs";
		private const string frontendLayoutName = "PageFrontend";
		private const string backendLayoutName = "PageBackend";
		private const string viewModelLayoutName = "PageViewModel";
		private const string comma = ", ";
		private const string subfoldersSeparator = "/";

		private string _pageFrontendFormat;
		private string _pageBackendFormat;
		private string _pageViewModelFormat;
		private readonly ProjectManager _projectManager;
		private readonly Project _project;
		private readonly DirectoryInfo _directory;
		private readonly string _directoryPath;
		private readonly string _pagesTargetPath;
		private readonly string _viewModelsTargetPath;

		[Required]
		public string Name { get; set; }
		[Required]
		[Setting]
		public string Namespace { get; set; }
		[Setting]
		public string Subfolders { get; set; }
		[Setting]
		public bool AllowGoingBackToPreviousPage { get; set; }

		public ICommand GenerateCommand { get; }

		public string FrontendFile => $"{Name}{frontendSuffix}";
		public string BackendFile => $"{Name}{backendSuffix}";
		public string ViewModelFile => $"{Name}{viewModelSuffix}";

		public PagesPageViewModel(ProjectManager projectManager)
		{
			Task.Run(LoadTemplates);

			_projectManager = projectManager;

			_project = _projectManager.CurrentProject;
			_directory = _project.Pages.Location;
			_directoryPath = _directory.FullName;
			_pagesTargetPath = _project.FileContent.PagesTargetPath;
			_viewModelsTargetPath = _project.FileContent.PageViewModelsTargetPath;

			GenerateCommand = new RelayCommand(OnGenerate);
		}

		private void OnGenerate()
		{
			if (!_validator.Validate()) return;

			_directory.GetFiles().ForEach(file => File.Delete(file.FullName));

			var frontend = _pageFrontendFormat.Format(Namespace, Name);
			var backend = _pageBackendFormat.Format(Namespace, Name);

			var subfolderList = Subfolders.Split(subfoldersSeparator, StringSplitOptions.RemoveEmptyEntries);

			var subfolders = subfolderList
				.Select(subfolder => subfolder.SurroundWith("\""))
				.Join(comma);

			var previousPage = AllowGoingBackToPreviousPage 
				? $"{nameof(PageAttribute.AllowGoingToPreviousPage)} = true"
				: "";

			var hasSubfolders = subfolders.IsNotNullOrEmpty();
			var hasPreviousPage = previousPage.IsNotNullOrEmpty();

			var separator = hasSubfolders && hasPreviousPage ? comma : "";

			var pageAttribute = hasSubfolders || hasPreviousPage
				? $"{Environment.NewLine}\t[Page({subfolders}{separator}{previousPage})]"
				: "";

			var viewModel = _pageViewModelFormat.Format(Namespace, Name, pageAttribute);

			var directory = Directory.CreateDirectory(Path.Combine(_directoryPath, Name)).FullName;

			File.WriteAllText(GetPath(subfolderList, directory, FrontendFile), frontend);
			File.WriteAllText(GetPath(subfolderList, directory, BackendFile), backend);
			File.WriteAllText(GetPath(subfolderList, directory, ViewModelFile), viewModel);

			if (!Directory.Exists(_pagesTargetPath))
			{
				Directory.CreateDirectory(_pagesTargetPath);
			}

			File.WriteAllText(GetPath(subfolderList, _pagesTargetPath, FrontendFile), frontend);
			File.WriteAllText(GetPath(subfolderList, _pagesTargetPath, BackendFile), backend);
			File.WriteAllText(GetPath(subfolderList, _viewModelsTargetPath, ViewModelFile), viewModel);
		}

		private string GetPath(string[] subfolders, string before, string after)
		{
			var paths = new string[subfolders.Length + 2];

			paths[0] = before;

			for (int i = 0; i < subfolders.Length; i++)
			{
				paths[i + 1] = subfolders[i];
			}

			paths[^1] = after;

			return Path.Combine(paths);
		}

		private async Task LoadTemplates()
		{
			_pageFrontendFormat = await EmbeddedResourcesUtilities.ReadAsync(frontendLayoutName);
			_pageBackendFormat = await EmbeddedResourcesUtilities.ReadAsync(backendLayoutName);
			_pageViewModelFormat = await EmbeddedResourcesUtilities.ReadAsync(viewModelLayoutName);
		}

		public override void OnLoaded(WindowViewModel windowVM, object pageData, PageViewModel previousPage)
		{
			base.OnLoaded(windowVM, pageData, previousPage);

			Namespace ??= Path.GetFileNameWithoutExtension(_projectManager?.CurrentProject?.FileContent?.CsprojFile?.FullName);

			// TODO: if some paths have been changed in settings (previous page), update them here as well
		}
	}
}
