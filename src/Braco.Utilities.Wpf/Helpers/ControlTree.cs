using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Helper class used for working with control tree.
	/// It also contains helper methods used for finding an ancestor or
	/// child of some object.
	/// </summary>
	public class ControlTree
	{
		/// <summary>
		/// Elements that were found.
		/// </summary>
		public IList<DependencyObject> Elements { get; } = new List<DependencyObject>();

		/// <summary>
		/// Creates an instance of the control tree.
		/// </summary>
		/// <param name="root">Root from which to find the elements.</param>
		public ControlTree(DependencyObject root)
		{
			FindElements(root);
		}

		private void FindElements(DependencyObject root)
		{
			Elements.Add(root);

			var contentProperty = root.GetType().GetProperty(nameof(ContentControl.Content));

			if (contentProperty != null)
			{
				if (contentProperty.GetValue(root) is DependencyObject obj)
					FindElements(obj);
			}
			else
			{
				switch (root)
				{
					case Panel panel:
						foreach (var child in panel.Children)
							if(child is DependencyObject dependencyObject)
								FindElements(dependencyObject);
						break;

					case Decorator decorator:
						FindElements(decorator.Child);
						break;
				}
			}
		}

		/// <summary>
		/// Gets all the children of specified type from given parent.
		/// </summary>
		/// <typeparam name="T">Type of child to look for.</typeparam>
		/// <param name="parent">Parent from which to start the search.</param>
		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
		{
			if (parent == null)
				throw new ArgumentNullException(nameof(parent));

			var queue = new Queue<DependencyObject>(new[] { parent });

			while (queue.Any())
			{
				var reference = queue.Dequeue();
				var count = VisualTreeHelper.GetChildrenCount(reference);

				for (var i = 0; i < count; i++)
				{
					var child = VisualTreeHelper.GetChild(reference, i);

					if (child is T children)
						yield return children;

					queue.Enqueue(child);
				}
			}
		}

		/// <summary>
		/// Returns the first ancester of specified type.
		/// </summary>
		public static T FindAncestor<T>(DependencyObject current)
		    where T : DependencyObject
		{
			current = VisualTreeHelper.GetParent(current);

			while (current != null)
			{
				if (current is T target)
					return target;

				current = VisualTreeHelper.GetParent(current);
			};
			return null;
		}

		/// <summary>
		/// Returns a specific ancester of an object.
		/// </summary>
		public static T FindAncestor<T>(DependencyObject current, T lookupItem)
		    where T : DependencyObject
		{
			while (current != null)
			{
				if (current is T target && Equals(target, lookupItem))
					return target;

				current = VisualTreeHelper.GetParent(current);
			};

			return null;
		}

		/// <summary>
		/// Finds an ancestor object by name and type.
		/// </summary>
		public static T FindAncestor<T>(DependencyObject current, string parentName)
		    where T : DependencyObject
		{
			while (current != null)
			{
				if (parentName.IsNotNullOrEmpty())
				{
                    if (current is T target && current is FrameworkElement frameworkElement && frameworkElement.Name == parentName)
                        return target;
                }
				else if (current is T target)
				{
					return target;
				}

				current = VisualTreeHelper.GetParent(current);
			};

			return null;

		}

		/// <summary>
		/// Looks for a child control within a parent by name.
		/// </summary>
		public static T FindChild<T>(DependencyObject parent, string childName)
		    where T : DependencyObject
		{
			// Confirm parent and childName are valid.
			if (parent == null) return null;

			T foundChild = null;

			var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				// If the child is not of the request child type child
				var childType = child as T;
				if (childType == null)
				{
					// recursively drill down the tree
					foundChild = FindChild<T>(child, childName);

					// If the child is found, break so we do not overwrite the found child.
					if (foundChild != null) break;
				}
				else if (childName.IsNotNullOrEmpty())
				{
					// If the child's name is set for search
					if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
					{
						// if the child's name is of the request name
						foundChild = (T)child;
						break;
					}
					else
					{
						// recursively drill down the tree
						foundChild = FindChild<T>(child, childName);

						// If the child is found, break so we do not overwrite the found child.
						if (foundChild != null) break;
					}
				}
				else
				{
					// child element found.
					foundChild = (T)child;
					break;
				}
			}

			return foundChild;
		}

		/// <summary>
		/// Looks for a child control within a parent by type.
		/// </summary>
		public static T FindChild<T>(DependencyObject parent)
			where T : DependencyObject
		{
			// Confirm parent is valid.
			if (parent == null) return null;

			T foundChild = null;

			var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				// If the child is not of the request child type child
				if (!(child is T))
				{
					// recursively drill down the tree
					foundChild = FindChild<T>(child);

					// If the child is found, break so we do not overwrite the found child.
					if (foundChild != null) break;
				}
				else
				{
					// child element found.
					foundChild = (T)child;
					break;
				}
			}

			return foundChild;
		}
	}
}