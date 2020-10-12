using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Braco.Utilities.Extensions
{
	/// <summary>
	/// Extensions that can be used on different collections.
	/// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Top level of recursion when using <see cref="LoopThroughHierarchy{T}(IList{T}, Func{T, T, int, IList{T}, int, bool})"/>.
        /// </summary>
        public const int TopLevel = 1;

		/// <summary>
		/// Checks if collection is not null and has some elements.
		/// </summary>
		/// <typeparam name="T">Type used in collection.</typeparam>
		/// <param name="collection">Collection to check.</param>
		/// <returns></returns>
		public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
			=> !IsNullOrEmpty(collection);

        /// <summary>
        /// Checks if collection is null or has no elements.
        /// </summary>
        /// <typeparam name="T">Type used in collection.</typeparam>
        /// <param name="collection">Collection to check.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
            => collection == null || collection.Count() == 0;

        /// <summary>
        /// Checks if collection is null or has no elements.
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this IEnumerable collection)
        {
            if (collection == null)
                return true;

            // Get the enumerator for the collection
            var enumerator = collection.GetEnumerator();

            // If we can't move to the next element, there is nothing in there
            return enumerator.MoveNext() == false;
        }

		/// <summary>
		/// Checks if collection is not null and has some elements.
		/// </summary>
		/// <param name="collection">Collection to check.</param>
		/// <returns></returns>
		public static bool IsNotNullOrEmpty(this IEnumerable collection)
			=> !IsNullOrEmpty(collection);

        /// <summary>
        /// Loops through the collection and performs
        /// the <paramref name="action"/> on each item.
        /// </summary>
        /// <typeparam name="T">Type of items in the collection.</typeparam>
        /// <param name="collection">Collection to loop through.</param>
        /// <param name="action">Action to perform on each element of the colleciton.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
            => collection.ForEach((item, _) => action?.Invoke(item));

        /// <summary>
        /// Loops through the collection and performs
        /// the <paramref name="action"/> on each item.
        /// </summary>
        /// <typeparam name="T">Type of items in the collection.</typeparam>
        /// <param name="collection">Collection to loop through.</param>
        /// <param name="action">Action to perform on each element of the colleciton.
        /// The second parameter that is given is index of the item in the collection.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            if (collection.IsNullOrEmpty())
                return;

            var i = 0;
            foreach (var item in collection)
                action?.Invoke(item, i++);
        }

        /// <summary>
		/// Generic implementation for recursively going through a hierarchy of parent-children structure.
		/// If the type (<typeparamref name="T"/>) isn't a <see cref="IHierarchy{T}"/>, this will only
		/// loop through the collection.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">Initial collection.</param>
		/// <param name="recursion">Method that will be executed every time a new item is found. The provided data is:
		/// current item, parent of the current item, index in the loop through current collection, current collection and level of recursion.
		/// The returned bool should signal if the recursion should be continued for the given item.</param>
		/// <returns>Items in the hierarchy through which the method has gone through.</returns>
		public static IList<T> LoopThroughHierarchy<T>(this IList<T> collection, Func<T, T, int, IList<T>, int, bool> recursion)
		{
			// Keep track of what we've gone through
			var history = new List<T>();

			// Check if the type defines that the collection is a hierarchy
			var isHierarchy = typeof(IHierarchy<T>).IsAssignableFrom(typeof(T));

			// Define the top level
			var level = TopLevel;

			void Recursion(IList<T> items)
			{
				// Increase the level
				level++;

				for (int i = 0; i < items.Count; i++)
				{
					var current = items[i];
					history.Add(current);

					// If the collection is a hierarchy...
					if (isHierarchy)
					{
						// Find the parent in the history, we had to have hit it at some point
						// -or- it just doesn't have a parent (which is the case for the initial collection)
						var parent = history.FirstOrDefault(item => (item as IHierarchy<T>).IsParentOf(current));

						// If the recursion should proceed...
						if (recursion(current, parent, i, items, level))
						{
							var children = (current as IHierarchy<T>).GetChildren();

							// If there are children to the current item...
							if (children?.Count > 0)
								// Continue looping through
								Recursion(children);
						}
					}

					// Otherwise...
					else
					{
						// Just pass in the current item
						recursion(current, default, i, items, level);
					}
				}

				// We passed the current level, decrease the value
				level--;
			}

			// Start the recursion with the initial collection
			Recursion(collection);

			// Return everything we've looped through
			return history;
        }

        /// <summary>
        /// Generic implementation for recursively going through a hierarchy of parent-children structure.
        /// If the type (<typeparamref name="T"/>) isn't a <see cref="IHierarchy{T}"/>, this will only
        /// loop through the collection.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="collection">Initial collection.</param>
        /// <param name="onItem">Method that will be executed every time a new item is found. The provided data is:
        /// current item and level of recursion.</param>
        /// <returns>Items in the hierarchy through which the method has gone through.</returns>
        public static IList<T> LoopThroughHierarchy<T>(this IList<T> collection, Action<T, int> onItem)
            => collection.LoopThroughHierarchy((current, parent, index, currentCollection, level) => {
                onItem?.Invoke(current, level);
                return true;
            });

        /// <summary>
        /// Goes through hierarchy and tries to locate the given item.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="items">Initial collection.</param>
        /// <param name="toFind">Item to locate in the hierarchy.</param>
        /// <returns>Collection in which the item resides and at which index.</returns>
        public static (IList<T> collection, int index) FindMatchInHierarchy<T>(this IList<T> items, T toFind)
        {
            // Define the default result
            (IList<T> collection, int index) result = (null, -1);
            var found = false;

            items.LoopThroughHierarchy((current, parent, index, collection, level) =>
            {
                // Don't loop if it was found
                if (found)
                {
                    return false;
                }

                if (Equals(current, toFind))
                {
                    result.collection = collection;
                    result.index = index;
                    found = true;
                    return false;
                }

                return true;
            });

            return result;
        }

		/// <summary>
		/// Tries to find the parent using the <see cref="object.Equals(object, object)"/>
		/// for comparison.
		/// </summary>
		/// <typeparam name="T">Type used in collection.</typeparam>
		/// <param name="items">Collection where the item should exist.</param>
		/// <param name="item">Item to find in the collection.</param>
		/// <returns>Item from the collection, if found.</returns>
		public static T FindParent<T>(this IList<T> items, T item)
        {
            if (!(items?.Count > 0)) return default;

            T itemParent = default;
            var found = false;

            items.LoopThroughHierarchy((current, parent, index, collection, level) =>
            {
                if (found) return false;

                if (Equals(current, item))
                {
                    itemParent = parent;
                    found = true;
                    return false;
                }

                return true;
            });

            return itemParent;
        }

        /// <summary>
        /// Picks a random element from the collection
        /// </summary>
        /// <typeparam name="T">Type used in collection.</typeparam>
        /// <param name="collection">Collection from which the element will be picked.</param>
        /// <returns></returns>
        public static T RandomElement<T>(this IEnumerable<T> collection)
            => collection.IsNullOrEmpty()
                ? default
                : collection.ElementAt(NumberUtilities.Rng.Next(collection.Count()));

		/// <summary>
		/// Finds index of item that matches the given <paramref name="predicate"/>.
		/// </summary>
		/// <typeparam name="T">Type used in collection.</typeparam>
		/// <param name="collection">Collection to test.</param>
		/// <param name="predicate">Test that is used for finding the index.</param>
		/// <returns>Index of the item, if it exists. Otherwise -1.</returns>
		public static int IndexOf<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            for (int i = 0; i < collection.Count(); i++)
                if (predicate(collection.ElementAt(i)))
                    return i;

            return -1;
        }

		/// <summary>
		/// Gets index of the item in the collection by using
		/// <see cref="object.Equals(object, object)"/> as comparison.
		/// </summary>
		/// <typeparam name="T">Type used in collection.</typeparam>
		/// <param name="collection">Collection in which the item might be.</param>
		/// <param name="item">Item whose index needs to be found.</param>
		/// <returns>Index of the item, if it exists. Otherwise -1.</returns>
		public static int IndexOf<T>(this IEnumerable<T> collection, T item)
            => collection.IndexOf(collectionItem => Equals(item, collectionItem));

		/// <summary>
		/// Adds an item into the collection at given index. If the index
		/// is outside the bounds of the collection, item will just be added
		/// using <see cref="IList.Add(object)"/> method.
		/// </summary>
		/// <typeparam name="T">Type used in collection.</typeparam>
		/// <param name="list">List used for inserting the item.</param>
		/// <param name="index">Index at which to insert the item.</param>
		/// <param name="item">Item to insert.</param>
		public static void AddOrInsert<T>(this IList<T> list, int index, T item)
            => AddOrInsert((IList)list, index, item);

		/// <summary>
		/// Adds an item into the collection at given index. If the index
		/// is outside the bounds of the collection, item will just be added
		/// using <see cref="IList.Add(object)"/> method.
		/// </summary>
		/// <param name="list">List used for inserting the item.</param>
		/// <param name="index">Index at which to insert the item.</param>
		/// <param name="item">Item to insert.</param>
		public static void AddOrInsert(this IList list, int index, object item)
        {
            if (list == null) return;

            if (list.Count > 0 && index.Between(0, list.Count, InclusionOptions.OnlyLeftInclusive))
            {
                list.Insert(index, item);
            }
            else
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Takes in a collection, removes old items from it and adds new items to it.
        /// </summary>
        /// <typeparam name="T">Type of item in the collection.</typeparam>
        /// <param name="collection">Collection to renew.</param>
        /// <param name="newItems">Items to use to populate the collection.</param>
        /// <returns></returns>
        public static ICollection<T> RenewData<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            collection.Clear();

            newItems.ForEach(item => collection.Add(item));

            return collection;
        }

        /// <summary>
        /// Generates the next integer id based on the ids in the given collection.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="collection">Collection from which to read already present ids.</param>
        /// <param name="idGetter">Function that returns id of the item of type <typeparamref name="T"/>.</param>
        /// <returns>Next id based on the collection.</returns>
        public static int GetNextId<T>(this IEnumerable<T> collection, Func<T, int> idGetter)
        {
            var id = 1;

            if (collection.IsNotNullOrEmpty())
                id = collection.Max(idGetter) + 1;

            return id;
        }

        /// <summary>
        /// Checks if the item is inside a collection of values.
        /// </summary>
        /// <typeparam name="T">Type to check.</typeparam>
        /// <param name="item">Item to check.</param>
        /// <param name="targetValues">Collection to check the item against.</param>
        public static bool In<T>(this T item, params T[] targetValues)
        {
            if (item == null) throw new NullReferenceException();
            if (targetValues == null) throw new ArgumentNullException(nameof(targetValues));
			
            // Return whether or not the item is in the collection
            return targetValues.Contains(item);
        }

		/// <summary>
		/// Shorthand for calling <see cref="string.Join{T}(string, IEnumerable{T})"/>.
		/// </summary>
		/// <typeparam name="T">Type of object.</typeparam>
		/// <param name="collection">Collection from which to get strings.</param>
		/// <param name="separator">Separator used for members of the collection.</param>
		/// <returns>Members of the collection separated by <paramref name="separator"/>.</returns>
		public static string Join<T>(this IEnumerable<T> collection, string separator)
			=> string.Join(separator, collection);
    }
}
