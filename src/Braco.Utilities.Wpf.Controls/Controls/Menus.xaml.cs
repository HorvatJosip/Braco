using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for Menu.xaml
	/// </summary>
	public partial class Menus : UserControl
	{
		/// <summary>
		/// Collection of items for the menu
		/// </summary>
		public IEnumerable<MenuViewModel> Collection
		{
			get { return (IEnumerable<MenuViewModel>)GetValue(MenuItemsProperty); }
			set { SetValue(MenuItemsProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Collection"/>.
		/// </summary>
		public static readonly DependencyProperty MenuItemsProperty =
			DependencyProperty.Register(nameof(Collection), typeof(IEnumerable<MenuViewModel>), typeof(Menus), new PropertyMetadata(null));

		/// <summary>
		/// Creates an instance of the control.
		/// </summary>
		public Menus()
		{
			InitializeComponent();
		}
	}
}
