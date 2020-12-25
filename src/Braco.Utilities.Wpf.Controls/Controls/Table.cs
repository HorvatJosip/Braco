using Braco.Utilities.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// <see cref="DataGrid"/> that binds to <see cref="DataManager{T}"/>.
	/// </summary>
	public class Table : DataGrid
	{
		private const string
			dataManagerPropertyName = nameof(TableViewModel<int>.DataManager),
			editStartedCommandPropertyName = nameof(TableViewModel<int>.EditStartedCommand),
			editEndedCommandPropertyName = nameof(TableViewModel<int>.EditEndedCommand),
			sourceDataPropertyName = nameof(DataManager<int>.PageItems);

		private IEnumerable<Image> _images;

		public Table()
		{
			Loaded += Table_Loaded;
		}

		private void Table_Loaded(object sender, RoutedEventArgs e)
		{
			ICommand editStartedCommand = null, editEndedCommand = null;
			DataManager dataManager;
			Page page = null;

			void ExtractDataManager(object source)
			{
				dataManager = source?.GetType().GetProperty(dataManagerPropertyName)?.GetValue(source) as DataManager;
			}

			void ExtractCommands(object source)
			{
				editStartedCommand = source?.GetType().GetProperty(editStartedCommandPropertyName)?.GetValue(source) as ICommand;
				editEndedCommand = source?.GetType().GetProperty(editEndedCommandPropertyName)?.GetValue(source) as ICommand;
			}

			ExtractDataManager(DataContext);

			if (dataManager != null)
			{
				ExtractCommands(DataContext);
			}
			else
			{
				page = ControlTree.FindAncestor<Page>(this);

				ExtractDataManager(page?.DataContext);
			}

			if (dataManager == null) return;

			if (editStartedCommand == null && editEndedCommand == null)
			{
				ExtractCommands(page.DataContext);
			}

			ItemsSource = (IEnumerable)dataManager.GetType().GetProperty(sourceDataPropertyName).GetValue(dataManager);

			foreach (var column in dataManager.ColumnInfos)
			{
				Columns[column.DisplayIndex.Value].Header = FindResource(column.LocalizationKey);
			}

			if (editStartedCommand != null)
			{
				PreparingCellForEdit += (sender, e) => editStartedCommand.Execute(e.Row.Item);
			}
			if (editEndedCommand != null)
			{
				CellEditEnding += (sender, e) => editEndedCommand.Execute(e.Row.Item);
			}

			dataManager.DataUpdated += DataManager_DataUpdated;
		}

		private void DataManager_DataUpdated(object sender, PageDataEventArgs e)
		{
			if(_images == null)
			{
				var headers = ControlTree.FindVisualChildren<DataGridColumnHeader>(this).Skip(1);

				_images = headers.Select(header => ControlTree.FindChild<Image>(header));
			}

			_images.ForEach(image => image.UpdateSource());
		}
	}
}
