using Braco.Utilities.Extensions;
using System.Collections.Generic;
using System.Windows.Input;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Base view model for content that contains a table.
	/// </summary>
	/// <typeparam name="T">Type to display inside the table.</typeparam>
	public abstract class TableViewModel<T> : ContentViewModel
	{
		/// <summary>
		/// Data to display in the table.
		/// </summary>
		public DataManager<T> DataManager { get; }

		#region Commands

		/// <summary>
		/// Used for sorting data in the table.
		/// </summary>
		public ICommand TableSortCommand { get; }

		/// <summary>
		/// Called when row edit starts.
		/// </summary>
		public ICommand EditStartedCommand { get; }

		/// <summary>
		/// Called when row edit ends.
		/// </summary>
		public ICommand EditEndedCommand { get; }

		#endregion

		#region Constructor

		/// <summary>
		/// Sets up the data manager and virtual method calls.
		/// </summary>
		protected TableViewModel()
		{
			DataManager = new DataManager<T>(_localizer, Load());

			DataManager.PageChanged += (sender, e) => PageChanged(e);
			DataManager.PageSizeChanged += (sender, e) => PageSizeChanged(e);
			DataManager.NumPagesChanged += (sender, e) => NumPagesChanged(e);

			if (int.TryParse(_config[ConfigurationKeys.PageSizeKey], out var pageSize))
			{
				DataManager.PageSize = pageSize;
			}

			DataManager.Page = 1;

			TableSortCommand = new RelayCommand<List<object>>(OnTableSort);
			EditStartedCommand = new RelayCommand<T>(OnEditStarted);
			EditEndedCommand = new RelayCommand<T>(OnEditEnded);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Used for loading the data.
		/// </summary>
		/// <returns>Data that will be displayed in the grid.</returns>
		protected abstract IEnumerable<T> Load();

		/// <summary>
		/// Called when one of the columns was sorted.
		/// <para>By default extracts column name from <paramref name="parameters"/> and sorts using
		/// the <see cref="DataManager.Sort(string)"/> method.</para>
		/// </summary>
		/// <param name="parameters">Parameters for the sort command.</param>
		protected virtual void OnTableSort(List<object> parameters)
		{
			if (parameters.IsNotNullOrEmpty() && parameters.Count > 1 && parameters[1] is string column)
			{
				DataManager.Sort(column);
			}
		}

		/// <summary>
		/// Called when a row starts editing.
		/// <para>Note: base implementation does nothing.</para>
		/// </summary>
		/// <param name="item">Item that represents a row that has started editing.</param>
		protected virtual void OnEditStarted(T item) { }

		/// <summary>
		/// Called when a row edit ends.
		/// <para>Note: base implementation does nothing.</para>
		/// </summary>
		/// <param name="item">Item that represents a row that has finished editing.</param>
		protected virtual void OnEditEnded(T item) { }

		/// <summary>
		/// Called when the table page changes.
		/// <para>Note: base implementation does nothing.</para>
		/// </summary>
		/// <param name="e">Page data information.</param>
		protected virtual void PageChanged(PageDataEventArgs e) { }

		/// <summary>
		/// Called when the table page size changes.
		/// <para>Note: base implementation does nothing.</para>
		/// </summary>
		/// <param name="e">Page data information.</param>
		protected virtual void PageSizeChanged(PageDataEventArgs e) { }

		/// <summary>
		/// Called when the number of pages changes inside the table.
		/// <para>Note: base implementation does nothing.</para>
		/// </summary>
		/// <param name="e">Page data information.</param>
		protected virtual void NumPagesChanged(PageDataEventArgs e) { }

		#endregion
	}
}
