using Braco.Services;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Page that sets up the view model as DataContext.
    /// </summary>
    /// <typeparam name="VM">View model to use for DataContext.</typeparam>
    public class BasePage<VM> : Page where VM : ContentViewModel
    {
		/// <summary>
		/// Creates an instance of the page.
		/// </summary>
        public BasePage()
        {
            // Setup the data context using the view model
            DataContext = DI.Get<VM>() ?? System.Activator.CreateInstance<VM>();
        }
    }
}
