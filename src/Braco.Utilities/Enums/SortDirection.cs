namespace Braco.Utilities
{
    /// <summary>
    /// Defines directions in which items can be sorted.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// The state where sort isn't active. If this is set on a column, icon that represents
        /// sorting in ascending order should be shown.
        /// </summary>
        None,

        /// <summary>
        /// The state where sort is active and the items are sorted in ascending order.
        /// If this is set on a column, icon that represents sorting in descending order should be shown.
        /// </summary>
        Ascending,

        /// <summary>
        /// The state where sort is active and the items are sorted in descending order.
        /// If this is set on a column, either icon that represents sorting in ascending order 
        /// should be shown or an icon that represents resetting the sorting to default sort.
        /// </summary>
        Descending
    }
}
