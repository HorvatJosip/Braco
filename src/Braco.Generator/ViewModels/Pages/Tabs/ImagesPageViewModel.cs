using Braco.Utilities;
using Braco.Utilities.Extensions;
using Braco.Utilities.Wpf;
using Braco.Utilities.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Braco.Generator
{
	[Page("Tabs")]
	public class ImagesPageViewModel : PageViewModel
	{
		private const string imageFilters = "*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tif;*.svg";

		private readonly IChooserDialogsService _dialogs;
		private readonly ProjectManager _projectManager;

		public ObservableCollection<PreviewableImageViewModel> Images { get; set; }

		public ICommand AddNewImagesCommand { get; }
		public ICommand RemoveAllImagesCommand { get; }
		public ICommand ImportIntoProjectCommand { get; }
		public ICommand RemoveImageCommand { get; }
		public ICommand MouseEnteredImageCommand { get; }
		public ICommand MouseLeftImageCommand { get; }
		public ICommand DropCommand { get; }

		public ImagesPageViewModel(IChooserDialogsService dialogs, ProjectManager projectManager)
		{
			_dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
			_projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));

			AddNewImagesCommand = new RelayCommand(OnAddNewImages);
			RemoveAllImagesCommand = new RelayCommand(OnRemoveAllImages);
			ImportIntoProjectCommand = new RelayCommand(OnImportIntoProject);
			RemoveImageCommand = new RelayCommand<PreviewableImageViewModel>(OnRemoveImage);
			MouseEnteredImageCommand = new RelayCommand<PreviewableImageViewModel>(OnMouseEnteredImage);
			MouseLeftImageCommand = new RelayCommand<PreviewableImageViewModel>(OnMouseLeftImage);
			DropCommand = new RelayCommand<FileDropCommandRequest>(OnDrop);
		}

		private void OnAddNewImages()
		{
			var paths = _dialogs.ChooseFiles("Pick new images to add", ("Image files", imageFilters), ("All files", "*.*"));

			if (paths.IsNullOrEmpty()) return;

			AddMultipleImages(paths);
		}

		private void OnRemoveAllImages()
		{
			for (int i = Images.Count - 1; i >= 0; i--)
			{
				OnRemoveImage(Images[i]);
			}
		}

		private void OnImportIntoProject()
		{
			var existingPaths = new List<string>();

			Images.ForEach(image =>
			{
				var targetPath = GetTargetPath(image.Path, csProjDirectory: true);

				if (File.Exists(targetPath))
				{
					existingPaths.Add(targetPath);
					return;
				}

				File.Copy(image.Path, targetPath);
			});

			if (existingPaths.IsNotNullOrEmpty())
			{
				Dialog.Open(new DialogContent("Some images already exist", existingPaths.Join(", ")));
			}
		}

		private void OnRemoveImage(PreviewableImageViewModel image)
		{
			static bool Remove(string path)
			{
				if (!File.Exists(path)) return true;

				try
				{
					File.Delete(path);
					return true;
				}
				catch (Exception ex)
				{
					Dialog.Open(new DialogContent("Failed to remove image...", $"An error occurred trying to remove the image:{Environment.NewLine}{ex.Message}"));
					return false;
				}
			}

			if
			(
				Remove(GetTargetPath(image.Path, csProjDirectory: true)) &&
				Remove(GetTargetPath(image.Path, csProjDirectory: false))
			)
			{
				this.Change(x => x.Images, images => images.Remove(image));
			}
		}

		private void OnMouseEnteredImage(PreviewableImageViewModel image)
		{
			image.ShowPreview = true;
		}

		private void OnMouseLeftImage(PreviewableImageViewModel image)
		{
			image.ShowPreview = false;
		}

		private void OnDrop(FileDropCommandRequest request)
		{
			AddMultipleImages(request.FilePaths);
		}

		private void AddMultipleImages(IEnumerable<string> images)
		{
			var existingPaths = new List<string>();

			images.ForEach(imagePath =>
			{
				if (!AddImage(imagePath))
				{
					existingPaths.Add(imagePath);
				}
			});

			if (existingPaths.IsNotNullOrEmpty())
			{
				Dialog.Open(new DialogContent("Some images already exist", existingPaths.Join(", ")));
			}
		}

		private bool AddImage(string path)
		{
			var targetPath = GetTargetPath(path, csProjDirectory: false);

			if (File.Exists(targetPath))
			{
				// Ask for override?
				return false;
			}

			File.Copy(path, targetPath);

			this.Change(x => x.Images, images => images.Add(PreviewableImageViewModel.FromPath(path)));

			return true;
		}

		private string GetTargetPath(string imagePath, bool csProjDirectory)
			=> Path.Combine
			(
				csProjDirectory
					? _projectManager.CurrentProject.FileContent.ImagesTargetPath
					: _projectManager.CurrentProject.Images.Location.FullName,
				Path.GetFileName(imagePath)
			);
	}
}
