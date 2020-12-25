using Braco.Utilities.Extensions;
using Braco.Services;
using Braco.Utilities.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.IO;

namespace Braco.Generator
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Initializer.RunSpecificInitializer<AppInitializer>();

			FrameManagerDefinitions.InitializeFromConfiguration(DI.ReadOnlyConfiguration);

			StyleUtilities.OverrideStyles(typeof(System.Windows.Window), typeof(Page));

			var windowsManager = DI.Get<IWindowsManager>();

			windowsManager.Open<MainWindowViewModel, WelcomePageViewModel>();

			TypeDescriptor.AddAttributes(typeof(ObservableCollection<FileViewModel>), new TypeConverterAttribute(typeof(ExistingProjectsConverter)));

			DI.Configuration.Load();

			while (!DI.Localizer.ChangeLanguage(DI.Configuration[ConfigurationKeys.Language])) ;

			Current.MainWindow = (windowsManager.ActiveWindow as Utilities.Wpf.Window).UI;
			Current.MainWindow.ShowDialog();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			DI.Configuration.Save();
			var project = DI.Get<ProjectManager>()?.CurrentProject;

			if (project != null)
			{
				project.Save();

				DI.Get<LocalizationPageViewModel>().CultureLocalizations.ForEach(culture =>
				{
					new[] { false, true }
						.Select(isCSProj => isCSProj ? new DirectoryInfo(project.FileContent.LocalizationTargetPath) : project.Localization.Location)
						.ForEach(dir =>
						{
							var (data, directoryName) = CultureLocalizationSerializationData.FromCulture(culture);

							var sectionDirectory = Path.Combine(dir.FullName, directoryName);

							Directory.CreateDirectory(sectionDirectory);

							data.Sections.ForEach(section =>
							{
								if (section.Name.IsNullOrEmpty()) return;

								var file = Path.Combine(sectionDirectory, $"{section.Name}.json");

								File.WriteAllText(file, section.Json);
							});
						});
				});
			}

			base.OnExit(e);
		}
	}
}
