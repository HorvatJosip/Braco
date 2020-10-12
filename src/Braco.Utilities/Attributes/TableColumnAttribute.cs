using System;

namespace Braco.Utilities
{
    /// <summary>
    /// Specifies that the marked property is shown
    /// in a table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumnAttribute : Attribute
    {
        /// <summary>
        /// Name of the resource for getting localized header value.
        /// </summary>
        public string ResourceName { get; }

        /// <summary>
        /// Column index in the table.
        /// </summary>
        public int DisplayIndex { get; }

        /// <summary>
        /// Creates an instance of the <see cref="TableColumnAttribute"/>
        /// with specific resource name and display index.
        /// </summary>
        /// <param name="resourceName">Name of the resource for getting localized header value.</param>
        /// <param name="displayIndex">Column index in the table.</param>
        public TableColumnAttribute(string resourceName, int displayIndex)
        {
            ResourceName = resourceName;
            DisplayIndex = displayIndex;
        }
    }
}