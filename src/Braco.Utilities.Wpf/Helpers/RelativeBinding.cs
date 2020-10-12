using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Shorthand way for writing binding with relative type
	/// that searches for an ancestor. By default, searches
	/// for ancestor that is a <see cref="UserControl"/>.
	/// </summary>
	public class RelativeBinding : Binding
	{
		/// <summary>
		/// Prefix for a path representing something on DataContext.
		/// </summary>
		public const string DataContextPathPrefix = "DataContext.";

		private Type _ancestorType;

		/// <summary>
		/// Type of the ancestor to use.
		/// </summary>
		public Type AncestorType
		{
			get => _ancestorType;
			set
			{
				if(value != _ancestorType)
				{
					RelativeSource = new RelativeSource
					{
						Mode = RelativeSourceMode.FindAncestor,
						AncestorType = value
					};

					_ancestorType = value;
				}
			}
		}

		/// <summary>
		/// Creates a default instance of the binding.
		/// </summary>
		public RelativeBinding() { }

		/// <summary>
		/// Creates an instance of the binding that looks for
		/// <see cref="UserControl"/> at given <paramref name="path"/>.
		/// </summary>
		/// <param name="path">Path of the bound member.</param>
		public RelativeBinding(string path) : this(path, typeof(UserControl)) { }

		/// <summary>
		/// Creates an instance of the binding that looks for
		/// <paramref name="type"/> at given <paramref name="path"/>.
		/// </summary>
		/// <param name="path">Path of the bound member.</param>
		/// <param name="type">Type of the ancestor to look for.</param>
		public RelativeBinding(string path, Type type) : this(path, type, false) { }

		/// <summary>
		/// Creates an instance of the binding that looks for
		/// <paramref name="type"/> at given <paramref name="path"/> and
		/// optionally prepends data context prefix to the path.
		/// </summary>
		/// <param name="path">Path of the bound member.</param>
		/// <param name="type">Type of the ancestor to look for.</param>
		/// <param name="prependDataContext">Should <see cref="DataContextPathPrefix"/>
		/// be prepended to <paramref name="path"/>?</param>
		public RelativeBinding(string path, Type type, bool prependDataContext) 
			: base(prependDataContext ? $"{DataContextPathPrefix}{path}" : path)
			=> AncestorType = type;
	}
}
