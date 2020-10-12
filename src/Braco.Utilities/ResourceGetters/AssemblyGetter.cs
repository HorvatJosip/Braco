using Braco.Services.Abstractions;
using System;
using System.Reflection;

namespace Braco.Utilities
{
	/// <summary>
	/// <see cref="ResourceGetter"/> for assembly name.
	/// </summary>
	public class AssemblyGetter : ResourceGetter
	{
		/// <summary>
		/// Represents type from target assembly.
		/// </summary>
		protected readonly Type _typeFromAssembly;

		/// <summary>
		/// Create an instance of the <see cref="AssemblyGetter"/>.
		/// </summary>
		/// <param name="instanceFromAssembly">Instance that can be passed in from the
		/// assembly whose name should be extracted.</param>
		public AssemblyGetter(object instanceFromAssembly) 
			: this(instanceFromAssembly?.GetType() ?? throw new ArgumentNullException(nameof(instanceFromAssembly), "You must provide an instance in order to extract the name of the assembly that instance resides in.")) { }

		/// <summary>
		/// Create an instance of the <see cref="AssemblyGetter"/>.
		/// </summary>
		/// <param name="typeFromAssembly">Type from assembly whose name should be extracted.</param>
		public AssemblyGetter(Type typeFromAssembly)
		{
			_typeFromAssembly = typeFromAssembly ?? throw new ArgumentNullException(nameof(typeFromAssembly), "You must provide a type in order to extract the name of the assembly.");
		}

		/// <summary>
		/// Used for getting the target assembly.
		/// </summary>
		/// <returns>Assembly that the <see cref="_typeFromAssembly"/> resides in.</returns>
		public virtual Assembly GetAssembly()
			=> _typeFromAssembly.Assembly;

		/// <summary>
		/// Used for getting name of the assembly.
		/// </summary>
		/// <returns>Name of the assembly that the <see cref="_typeFromAssembly"/> resides in.</returns>
		public virtual string GetAssemblyName()
			=> _typeFromAssembly.Assembly.GetName().Name;
	}
}
