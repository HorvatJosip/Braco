using System.Collections.Generic;

namespace Braco.Utilities
{
    /// <summary>
    /// Defines what a type needs to do in order
    /// to allow for looping through its hierarchy.
    /// </summary>
    /// <typeparam name="T">Type that will be used for hierarchy operations.</typeparam>
    public interface IHierarchy<T>
    {
        /// <summary>
        /// Receives another item of type <typeparamref name="T"/> and
        /// checks if that item is child of the current type.
        /// </summary>
        /// <param name="item">Item to test if it is a child of
        /// the current item.</param>
        /// <returns>True if current item is parent of the given item.</returns>
        bool IsParentOf(T item);

        /// <summary>
        /// Gets children of the current item.
        /// </summary>
        /// <returns>Children collection.</returns>
        IList<T> GetChildren();
    }
}
