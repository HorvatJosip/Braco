using System;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Specifies certain information about a window.
	/// <para>Should be used on a <see cref="WindowViewModel"/>.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
    public class WindowAttribute : Attribute
    {
		/// <summary>
		/// Type of window to instantiate. Should inherit from <see cref="Window"/>.
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// Name of the type of window (e.g. MainWindow).
		/// </summary>
		public string TypeName { get; }

		/// <summary>
		/// Generates an instance of the attribute with given type.
		/// </summary>
		/// <param name="type">Type of window to instantiate. Should inherit <see cref="Window"/>.</param>
		public WindowAttribute(Type type)
		{
			Type = type;
		}

		/// <summary>
		/// Generates an instance of the attribute with type name.
		/// </summary>
		/// <param name="typeName">Name of the type of window (e.g. MainWindow).</param>
		public WindowAttribute(string typeName)
		{
			TypeName = typeName;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"{Type?.Name ?? TypeName ?? "WindowName"}";
	}
}
