using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Represents an item group.
	/// </summary>
	public class ItemGroup : ContentControl
	{
		/// <summary>
		/// Header for the item group.
		/// </summary>
		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Header"/>.
		/// </summary>
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register(nameof(Header), typeof(string), typeof(ItemGroup), new PropertyMetadata(null));
	}
}
