using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for ScrollableList.xaml
	/// </summary>
	public partial class ScrollableList : UserControl
	{
		/// <summary>
		/// Default template used for <see cref="ItemsPanelTemplate"/> that expects a panel
		/// as a format argument (see <see cref="DefaultItemsControlPanel"/> for example).
		/// </summary>
		public const string DefaultItemsControlTemplateFormat = @"
			<ItemsPanelTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
				{0}
			</ItemsPanelTemplate>
";

		/// <summary>
		/// Default panel used inside <see cref="ItemsPanelTemplate"/>.
		/// </summary>
		public const string DefaultItemsControlPanel = "<StackPanel HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Top\" />";

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
			DependencyProperty.Register(nameof(ListItems), typeof(IEnumerable), typeof(ScrollableList), new PropertyMetadata(null));

		/// <summary>
		/// Template of an item shown in the <see cref="ItemsControl.ItemTemplate"/>.
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
			DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(ScrollableList), new PropertyMetadata(null));


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
			DependencyProperty.Register(nameof(ListPanelTemplate), typeof(ItemsPanelTemplate), typeof(ScrollableList), new PropertyMetadata(GetDefaultItemsPanelTemplate()));

		/// <summary>
		/// Visibility of horizontal <see cref="System.Windows.Controls.Primitives.ScrollBar"/>.
		/// </summary>
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty); }
			set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="HorizontalScrollBarVisibility"/>.
		/// </summary>
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty =
			DependencyProperty.Register(nameof(HorizontalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(ScrollableList), new PropertyMetadata(ScrollBarVisibility.Disabled));

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
			DependencyProperty.Register(nameof(VerticalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(ScrollableList), new PropertyMetadata(ScrollBarVisibility.Auto));

		/// <summary>
		/// Determines if the list should be focusable or not.
		/// </summary>
		public bool IsListFocusable
		{
			get { return (bool)GetValue(IsListFocusableProperty); }
			set { SetValue(IsListFocusableProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="IsListFocusable"/>.
		/// </summary>
		public static readonly DependencyProperty IsListFocusableProperty =
			DependencyProperty.Register(nameof(IsListFocusable), typeof(bool), typeof(ScrollableList), new PropertyMetadata(false));

		/// <summary>
		/// Should the <see cref="ScrollViewer"/> always scroll even when hover over different elements?
		/// </summary>
		public bool ShouldAlwaysScrollWithin
		{
			get { return (bool)GetValue(ShouldAlwaysScrollWithinProperty); }
			set { SetValue(ShouldAlwaysScrollWithinProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="ShouldAlwaysScrollWithin"/>.
		/// </summary>
		public static readonly DependencyProperty ShouldAlwaysScrollWithinProperty =
			DependencyProperty.Register(nameof(ShouldAlwaysScrollWithin), typeof(bool), typeof(ScrollableList), new PropertyMetadata(true));

		/// <summary>
		/// Gets <see cref="ItemsPanelTemplate"/> with stack panel that stacks items from top.
		/// </summary>
		/// <returns>Template for the items panel.</returns>
		public static ItemsPanelTemplate GetDefaultItemsPanelTemplate()
			=> GetItemsPanelTemplate(DefaultItemsControlPanel);

		/// <summary>
		/// Gets <see cref="ItemsPanelTemplate"/> with stack panel that stacks items from top.
		/// </summary>
		/// <param name="itemsControlPanel">Panel to use for the template (see <see cref="DefaultItemsControlPanel"/> for example).</param>
		/// <returns>Template for the items panel.</returns>
		public static ItemsPanelTemplate GetItemsPanelTemplate(string itemsControlPanel)
			=> (ItemsPanelTemplate)XamlReader.Parse(string.Format(DefaultItemsControlTemplateFormat, itemsControlPanel));

		/// <summary>
		/// Creates an instance of the control.
		/// </summary>
		public ScrollableList()
		{
			InitializeComponent();
		}
	}
}
