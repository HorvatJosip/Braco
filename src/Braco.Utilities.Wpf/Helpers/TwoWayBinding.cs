using System;
using System.Windows.Data;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Shorthand way for making a binding that binds two way
	/// and updates when the property changes.
	/// </summary>
	public class TwoWayBinding : RelativeBinding
	{
		/// <summary>
		/// Creates a default instance of the binding.
		/// </summary>
		public TwoWayBinding() { }

		/// <summary>
		/// Creates an instance of the binding that binds to
		/// member at given path using <see cref="BindingMode.TwoWay"/>.
		/// </summary>
		/// <param name="path">Path of the bound member.</param>
		public TwoWayBinding(string path) : this(path, null) { }

		/// <summary>
		/// Creates an instance of the binding that binds to
		/// member at given path using <see cref="BindingMode.TwoWay"/>
		/// and looks for the member in the ancestor of given
		/// <paramref name="type"/>.
		/// </summary>
		/// <param name="path">Path of the bound member.</param>
		/// <param name="type">Type of the ancestor to look for.</param>
		public TwoWayBinding(string path, Type type) 
			: this(path, type, false) { }

		/// <summary>
		/// Creates an instance of the binding that binds to
		/// member at given path using <see cref="BindingMode.TwoWay"/>
		/// and looks for the member in the ancestor of given
		/// <paramref name="type"/>. Optionally, it prepends
		/// data context prefix to the path.
		/// </summary>
		/// <param name="path">Path of the bound member.</param>
		/// <param name="type">Type of the ancestor to look for.</param>
		/// <param name="prependDataContext">Should <see cref="RelativeBinding.DataContextPathPrefix"/>
		/// be prepended to <paramref name="path"/>?</param>
		public TwoWayBinding(string path, Type type, bool prependDataContext) 
			: base(path, type, prependDataContext)
		{
			Mode = BindingMode.TwoWay;
			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
		}
	}
}
