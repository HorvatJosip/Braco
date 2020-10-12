using System;
using System.Collections.Generic;
using System.Reflection;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Used for setting up resource getters.
	/// </summary>
	public interface IResourceManager
	{
		/// <summary>
		/// Gets resource getter by type.
		/// </summary>
		/// <param name="resourceType">Type of resources to get.</param>
		/// <returns><see cref="ResourceGetter"/> instance, if it exists. Otherwise null.</returns>
		public ResourceGetter this[Type resourceType] { get; }

		/// <summary>
		/// Base location for all of the resources.
		/// </summary>
		string Location { get; set; }

		/// <summary>
		/// Used for separating multiple locations.
		/// </summary>
		string LocationSeparator { get; set; }

		/// <summary>
		/// Adds the resource getter if it isn't already added.
		/// </summary>
		/// <typeparam name="TResourceGetter">Type of resources.</typeparam>
		/// <param name="instance">Instance of the class that inherits <see cref="ResourceGetter"/>.
		/// <para>If left null, it will be created using the parameterless constructor.</para></param>
		/// <returns>True if added, false if there is already a getter for the given type.</returns>
		public bool Add<TResourceGetter>(TResourceGetter instance = null) where TResourceGetter : ResourceGetter;

		/// <summary>
		/// Adds all <see cref="ResourceGetter"/>s found in the given app domain.
		/// </summary>
		/// <param name="appDomain">App domain from which to add <see cref="ResourceGetter"/>s.</param>
		/// <returns>List of <see cref="ResourceGetter"/> types that were added.</returns>
		IList<Type> AddFromAppDomain(AppDomain appDomain);

		/// <summary>
		/// Adds all <see cref="ResourceGetter"/>s found in the given assembly.
		/// </summary>
		/// <param name="assembly">Assembly to use for type lookup.</param>
		/// <returns>List of <see cref="ResourceGetter"/> types that were added.</returns>
		IList<Type> AddFromAssembly(Assembly assembly);

		/// <summary>
		/// Adds the given resource getters to the collection.
		/// </summary>
		/// <param name="instances">Instances of resource getters.</param>
		/// <returns>Array that specifies which given getters were added and which were not,
		/// in the order in which they were passed in as parameters.</returns>
		bool[] AddRange(params object[] instances);

		/// <summary>
		/// Adds the given resource getters to the collection.
		/// </summary>
		/// <param name="resourceGetterTypes">Type list of <see cref="ResourceGetter"/>
		/// implementations.</param>
		/// <returns>Array that specifies which given getters were added and which were not,
		/// in the order in which they were passed in as parameters.</returns>
		bool[] AddRange(params Type[] resourceGetterTypes);

		/// <summary>
		/// Gets value from resource getter of given type.
		/// </summary>
		/// <typeparam name="TResourceGetter">Resource getter to use.</typeparam>
		/// <param name="parameters">Parameters that will be passed into the appropriate
		/// getter on the <see cref="ResourceGetter"/> implementation.</param>
		/// <returns>Value fetched by the getter.</returns>
		public object Get<TResourceGetter>(params object[] parameters) where TResourceGetter : ResourceGetter;

		/// <summary>
		/// Gets value from resources of given type.
		/// </summary>
		/// <typeparam name="TResourceGetter">Resource getter to use.</typeparam>
		/// <typeparam name="TResourceType">Return type for the getter method.</typeparam>
		/// <param name="parameters">Parameters that will be passed into the appropriate
		/// getter on the <see cref="ResourceGetter"/> implementation.</param>
		/// <returns>Value fetched by the getter.</returns>
		public TResourceType Get<TResourceGetter, TResourceType>(params object[] parameters) where TResourceGetter : ResourceGetter;

		/// <summary>
		/// Sets the resource getter for type <typeparamref name="TResourceGetter"/>.
		/// </summary>
		/// <typeparam name="TResourceGetter">Type of resources.</typeparam>
		/// <param name="instance">Instance of the class that inherits <see cref="ResourceGetter"/>.
		/// <para>If left null, it will be created using the parameterless constructor.</para></param>
		public void Set<TResourceGetter>(TResourceGetter instance = null) where TResourceGetter : ResourceGetter;

		/// <summary>
		/// Tries to set the resource getter for given type.
		/// </summary>
		/// <param name="resourceGetterType">Type of resource getter to set.</param>
		/// <returns>If the resource getter was added for given type.</returns>
		bool Set(Type resourceGetterType);

		/// <summary>
		/// Tries to set the resource getter for given type
		/// and add it using the given instance.
		/// </summary>
		/// <param name="resourceGetterType">Type of resource getter to set.</param>
		/// <param name="instance">Instance of the resource getter.</param>
		/// <returns>If the resource getter was added for given type.</returns>
		bool Set(Type resourceGetterType, object instance);
	}
}
