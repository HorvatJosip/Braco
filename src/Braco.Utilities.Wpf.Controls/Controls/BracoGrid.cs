using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// <see cref="Grid"/> with quick row and column definitions.
	/// </summary>
	public class BracoGrid : Grid
	{
		/// <summary>
		/// Defines what should be used for separating row / column definitions.
		/// </summary>
		public const string DefinitionsSeparator = ",";

		/// <summary>
		/// Defines what should be used for separating row / column multiplier specifications.
		/// </summary>
		public const string MultiplierSeparator = "x";

		/// <summary>
		/// Defines the pattern that the multiplier rows / columns need to match
		/// in order to be multiplied.
		/// </summary>
		public const string MultiplierPattern = @"\d+" + MultiplierSeparator;

		private static readonly GridLength _oneStarLength = new GridLength(1, GridUnitType.Star);

		/// <summary>
		/// Defines rows to use in the grid
		/// </summary>
		public string Rows
		{
			get { return (string)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Rows"/>.
		/// </summary>
		public static readonly DependencyProperty RowsProperty =
			DependencyProperty.Register(nameof(Rows), typeof(string), typeof(BracoGrid), new PropertyMetadata(null, new PropertyChangedCallback(OnRowsChanged)));

		private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is BracoGrid grid && e.NewValue is string)
			{
				grid.GenerateRowDefinitions();
			}
		}

		/// <summary>
		/// Defines Columns to use in the grid
		/// </summary>
		public string Columns
		{
			get { return (string)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Columns"/>.
		/// </summary>
		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.Register(nameof(Columns), typeof(string), typeof(BracoGrid), new PropertyMetadata(null, new PropertyChangedCallback(OnColumnsChanged)));

		private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is BracoGrid grid && e.NewValue is string)
			{
				grid.GenerateColumnDefinitions();
			}
		}

		/// <summary>
		/// Defines how should the rows be spaced in the grid.
		/// </summary>
		public Spaced SpacedRows
		{
			get { return (Spaced)GetValue(SpacedRowsProperty); }
			set { SetValue(SpacedRowsProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="SpacedRows"/>.
		/// </summary>
		public static readonly DependencyProperty SpacedRowsProperty =
			DependencyProperty.Register(nameof(SpacedRows), typeof(Spaced), typeof(BracoGrid), new PropertyMetadata(Spaced.None, new PropertyChangedCallback(OnSpacedRowsChanged)));

		private static void OnSpacedRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(d is BracoGrid grid && e.NewValue is Spaced)
			{
				grid.GenerateRowDefinitions();
			}
		}

		/// <summary>
		/// Defines how should the columns be spaced in the grid.
		/// </summary>
		public Spaced SpacedColumns
		{
			get { return (Spaced)GetValue(SpacedColumnsProperty); }
			set { SetValue(SpacedColumnsProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="SpacedColumns"/>.
		/// </summary>
		public static readonly DependencyProperty SpacedColumnsProperty =
			DependencyProperty.Register(nameof(SpacedColumns), typeof(Spaced), typeof(BracoGrid), new PropertyMetadata(Spaced.None, new PropertyChangedCallback(OnSpacedColumnsChanged)));

		private static void OnSpacedColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(d is BracoGrid grid && e.NewValue is Spaced)
			{
				grid.GenerateColumnDefinitions();
			}
		}

		private void GenerateDefinitions<T>(ICollection<T> collection, string value, Spaced spaced, Func<GridLength, T> creator /*Action<GridLength> adder, Action clear*/)
		{
			if (value.IsNullOrEmpty()) return;

			collection.Clear();

			if (spaced.Around())
			{
				collection.Add(creator(_oneStarLength));
			}

			var definitions = value.WithoutWhiteSpace().Split(DefinitionsSeparator).ToList();

			for (int i = 0; i < definitions.Count; i++)
			{
				var definition = definitions[i];

				if (spaced.Between() && i != 0)
				{
					collection.Add(creator(_oneStarLength));
				}

				if (Regex.IsMatch(definition, MultiplierPattern))
				{
					var split = definition.Split(MultiplierSeparator);

					var times = int.Parse(split[0]);

					var gridLength = split[1].Convert<GridLength>();

					for (int j = 0; j < times; j++)
					{
						if (spaced.Between() && j != 0)
						{
							collection.Add(creator(_oneStarLength));
						}

						collection.Add(creator(gridLength));
					}
				}
				else
				{
					collection.Add(creator(definition.Convert<GridLength>()));
				}
			}

			if (spaced.Around())
			{
				collection.Add(creator(_oneStarLength));
			}
		}

		private void GenerateRowDefinitions()
			=> GenerateDefinitions
			(
				collection: RowDefinitions,
				value: Rows,
				spaced: SpacedRows,
				creator: height => new RowDefinition { Height = height }
			);

		private void GenerateColumnDefinitions()
			=> GenerateDefinitions
			(
				collection: ColumnDefinitions,
				value: Columns,
				spaced: SpacedColumns,
				creator: width => new ColumnDefinition { Width = width }
			);
	}
}
