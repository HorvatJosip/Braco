using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for DropContainer.xaml
	/// </summary>
	public partial class DropContainer : UserControl
	{
		/// <summary>
		/// Name of the drop command that will be used by default.
		/// </summary>
		public const string DefaultDropCommandName = "DropCommand";

		/// <summary>
		/// Label to show if there are no items in the <see cref="ListItems"/>.
		/// </summary>
		public string EmptyLabel
		{
			get { return (string)GetValue(EmptyLabelProperty); }
			set { SetValue(EmptyLabelProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="EmptyLabel"/>.
		/// </summary>
		public static readonly DependencyProperty EmptyLabelProperty =
			DependencyProperty.Register(nameof(EmptyLabel), typeof(string), typeof(DropContainer), new PropertyMetadata(null));

		/// <summary>
		/// Defines if the <see cref="ListItems"/> should be shown as a <see cref="ScrollableList"/> or
		/// if the drop container should be shown with <see cref="EmptyLabel"/>.
		/// </summary>
		public bool ShowList
		{
			get { return (bool)GetValue(ShowListProperty); }
			set { SetValue(ShowListProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ShowList"/>.
		/// </summary>
		public static readonly DependencyProperty ShowListProperty =
			DependencyProperty.Register(nameof(ShowList), typeof(bool), typeof(DropContainer), new PropertyMetadata(false));

		/// <summary>
		/// Command to use for picking the files.
		/// </summary>
		public ICommand PickFilesCommand
		{
			get { return (ICommand)GetValue(PickFilesCommandProperty); }
			set { SetValue(PickFilesCommandProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="PickFilesCommand"/>.
		/// </summary>
		public static readonly DependencyProperty PickFilesCommandProperty =
			DependencyProperty.Register(nameof(PickFilesCommand), typeof(ICommand), typeof(DropContainer), new PropertyMetadata(null));

		/// <summary>
		/// Name of the drop command that will be used.
		/// </summary>
		public string DropCommandName
		{
			get { return (string)GetValue(DropCommandNameProperty); }
			set { SetValue(DropCommandNameProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="DropCommandName"/>.
		/// </summary>
		public static readonly DependencyProperty DropCommandNameProperty =
			DependencyProperty.Register(nameof(DropCommandName), typeof(string), typeof(DropContainer), new PropertyMetadata(DefaultDropCommandName));

		/// <summary>
		/// Paths to files that have been picked
		/// </summary>
		public IEnumerable ListItems
		{
			get { return (IEnumerable)GetValue(ListItemsProperty); }
			set { SetValue(ListItemsProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ListItems"/>.
		/// </summary>
		public static readonly DependencyProperty ListItemsProperty =
			DependencyProperty.Register(nameof(ListItems), typeof(IEnumerable), typeof(DropContainer), new PropertyMetadata(null));

		/// <summary>
		/// Template of an item shown in the <see cref="ItemsControl.ItemTemplate"/> when
		/// <see cref="ShowList"/> is set to true.
		/// </summary>
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ItemTemplate"/>.
		/// </summary>
		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(DropContainer), new PropertyMetadata(null));

		/// <summary>
		/// Template for the panel to use for the <see cref="ItemsControl"/>.
		/// </summary>
		public ItemsPanelTemplate ListPanelTemplate
		{
			get { return (ItemsPanelTemplate)GetValue(ListPanelTemplateProperty); }
			set { SetValue(ListPanelTemplateProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ListPanelTemplate"/>.
		/// </summary>
		public static readonly DependencyProperty ListPanelTemplateProperty =
			DependencyProperty.Register(nameof(ListPanelTemplate), typeof(ItemsPanelTemplate), typeof(DropContainer), new PropertyMetadata(ScrollableList.GetDefaultItemsPanelTemplate()));

		/// <summary>
		/// Visibility of horizontal <see cref="System.Windows.Controls.Primitives.ScrollBar"/>.
		/// </summary>
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty); }
			set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
		}

		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty =
			DependencyProperty.Register(nameof(HorizontalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(DropContainer), new PropertyMetadata(ScrollBarVisibility.Disabled));

		/// <summary>
		/// Visibility of vertical <see cref="System.Windows.Controls.Primitives.ScrollBar"/>.
		/// </summary>
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty); }
			set { SetValue(VerticalScrollBarVisibilityProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="VerticalScrollBarVisibility"/>.
		/// </summary>
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
			DependencyProperty.Register(nameof(VerticalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(DropContainer), new PropertyMetadata(ScrollBarVisibility.Auto));

		/// <summary>
		/// Determines if the list should be focusable or not.
		/// </summary>
		public bool IsItemListFocusable
		{
			get { return (bool)GetValue(IsItemListFocusableProperty); }
			set { SetValue(IsItemListFocusableProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="IsItemListFocusable"/>.
		/// </summary>
		public static readonly DependencyProperty IsItemListFocusableProperty =
			DependencyProperty.Register(nameof(IsItemListFocusable), typeof(bool), typeof(DropContainer), new PropertyMetadata(false));

		/// <summary>
		/// Creates an instance of the control.
		/// </summary>
		public DropContainer()
		{
			InitializeComponent();
		}
	}
}
