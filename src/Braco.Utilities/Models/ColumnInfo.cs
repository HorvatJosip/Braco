using System.Collections.Generic;
using System.Reflection;

namespace Braco.Utilities
{
    /// <summary>
    /// Information about a column.
    /// </summary>
    public class ColumnInfo
    {
		/// <summary>
		/// Name of the column.
		/// </summary>
        public string Name { get; }

		/// <summary>
		/// Property that this column is represented by.
		/// </summary>
        public PropertyInfo Property { get; }

		/// <summary>
		/// Sort direction of the column.
		/// </summary>
        public SortDirection SortDirection { get; set; }

		/// <summary>
		/// Is the column read-only?
		/// </summary>
        public bool IsReadonly { get; }

		/// <summary>
		/// Localized values for the column header.
		/// </summary>
        public IList<string> DisplayNames { get; set; }

		/// <summary>
		/// Index to use for this column.
		/// </summary>
        public int? DisplayIndex { get; set; }

		/// <summary>
		/// Key used for getting localized values.
		/// </summary>
        public string LocalizationKey { get; set; }

		/// <summary>
		/// Creates a new column info for the given property.
		/// </summary>
		/// <param name="property">property that this column is represented by.</param>
		public ColumnInfo(PropertyInfo property)
        {
            Property = property;
            Name = property.Name;
            IsReadonly = property.GetSetMethod()?.IsPublic != true;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Column for {Property} ({Name}: {(IsReadonly ? "R" : "Not r")}ead-only)";
	}
}