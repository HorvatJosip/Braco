using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for Pagination.xaml
	/// </summary>
	public partial class Pagination : UserControl
	{
		private const int size = 35;
		private const int margin = 5;

		private const int numPageButtons = 10;
		private const int numPagesDisplayedOnTheLeft = 5;
		private const int numPagesDisplayedOnTheRight = 4;

		private readonly Style buttonStyle;

		/// <summary>
		/// Defines how many pages are there.
		/// </summary>
		public int NumPages
		{
			get { return (int)GetValue(NumPagesProperty); }
			set { SetValue(NumPagesProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="NumPagesProperty"/>.
		/// </summary>
		public static readonly DependencyProperty NumPagesProperty =
			DependencyProperty.Register(nameof(NumPages), typeof(int), typeof(Pagination), new PropertyMetadata(new PropertyChangedCallback(OnNumPagesChanged)));

		/// <summary>
		/// Currently selected page.
		/// </summary>
		public int CurrentPage
		{
			get { return (int)GetValue(CurrentPageProperty); }
			set { SetValue(CurrentPageProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="CurrentPage"/>.
		/// </summary>
		public static readonly DependencyProperty CurrentPageProperty =
			DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(Pagination), new PropertyMetadata(new PropertyChangedCallback(OnCurrentPageChanged)));

		/// <summary>
		/// Creates an instance of the control.
		/// </summary>
		public Pagination()
		{
			InitializeComponent();

			FirstPageButtonPanel.Visibility = Visibility.Collapsed;

			FirstPageButton.Width = size;
			FirstPageButton.Height = size;
			FirstPageButton.Margin = new Thickness(margin);
			FirstPageButton.Style = buttonStyle;
			FirstPageButton.Command = new RelayCommand(() => CurrentPage = 1);
			FirstPageButton.Content = 1;

			LastPageButton.Width = size;
			LastPageButton.Height = size;
			LastPageButton.Margin = new Thickness(margin);
			LastPageButton.Style = buttonStyle;
		}

		private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not Pagination pagination || e.NewValue is not int page || page < 1)
				return;

			pagination.GeneratePageButtons();
		}

		private static void OnNumPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not Pagination pagination || e.NewValue is not int numPages || numPages < 1)
				return;

			if (pagination.CurrentPage == 1)
				pagination.GeneratePageButtons();
			else
				pagination.CurrentPage = 1;
		}

		private void GeneratePageButtons()
		{
			PageButtonsHolder.Children.Clear();

			var current = CurrentPage;
			var max = NumPages;

			var startPage = current - numPagesDisplayedOnTheLeft;
			var endPage = current + numPagesDisplayedOnTheRight;

			if (startPage < 1)
			{
				startPage = 1;
				endPage = numPageButtons;
			}

			FirstPageButtonPanel.Visibility = VisibilityHelpers.Convert(startPage > 1, null);

			if (endPage > max)
				endPage = max;

			LastPageButtonPanel.Visibility = VisibilityHelpers.Convert(endPage < max, null);
			LastPageButton.Command = new RelayCommand(() => CurrentPage = max);
			LastPageButton.Content = max;

			if (max >= numPageButtons && endPage - startPage != (numPageButtons - 1))
				startPage = endPage - (numPageButtons - 1);

			for (int i = startPage; i <= endPage; i++)
			{
				var pageNum = i;

				if (pageNum >= 1 && pageNum <= max)
					PageButtonsHolder.Children.Add(new Button
					{
						Content = pageNum,
						Style = buttonStyle,
						Width = size,
						Height = size,
						Margin = new Thickness(margin),
						Command = new RelayCommand(() => CurrentPage = pageNum)
					});
			}

			ChangeCurrentPage();
		}

		private void ChangeCurrentPage()
		{
			var buttons = ControlTree.FindVisualChildren<Button>(PageButtonsHolder);

			for (int i = 0; i < Math.Min(NumPages, buttons.Count()); i++)
			{
				var button = buttons.ElementAt(i);

				IsSelectedProperty.SetValue(button, CurrentPage == (int)button.Content);
			}
		}
	}
}
