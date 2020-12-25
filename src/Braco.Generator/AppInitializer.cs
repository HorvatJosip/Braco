using Braco.Services;
using Braco.Utilities.Wpf;

namespace Braco.Generator
{
	class AppInitializer : Initializer
	{
		public AppInitializer() : base(addEverything: false)
		{
			AddServiceSetups
			(
				// Braco.Services
				typeof(FileConfigurationServiceSetup),
				typeof(FileManagerSetup),
				typeof(MapperSetup),
				typeof(ResourceManagerSetup),
				typeof(SecurityServiceSetup),
				typeof(WindowsProcessStarterSetup),

				// Braco.Utilities.Wpf
				typeof(ChooserDialogsServiceSetup),
				typeof(ImageGetterSetup),
				typeof(LocalizedCollectionsSetup),
				typeof(ToolTipGetterSetup),
				typeof(ViewModelsSetup),
				typeof(WindowsManagerSetup),
				typeof(WpfDictionaryLocalizerSetup),
				typeof(WpfMethodServiceSetup),

				// Custom
				typeof(AssemblyGetterSetup),
				typeof(ProjectManagerSetup),
				typeof(MainWindowViewModelSetup)
			);

			AddPostServiceBuildInitializers(typeof(ResourceManagerInitializer));
		}
	}
}
