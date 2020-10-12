using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Base class for fetching logic of a specific resource.
	/// </summary>
	public abstract class ResourceGetter
	{
		/// <summary>
		/// If a method that inherits this class starts with this string,
		/// it will be placed into the collection of available methods for
		/// getting a resource.
		/// </summary>
		public const string GetterMethodPrefix = "Get";

		/// <summary>
		/// Default flags that will be used for getting methods for fetching
		/// resources in the inheriting class.
		/// </summary>
		public const BindingFlags DefaultGetterFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

		/// <summary>
		/// List of getters that are defined in the current class.
		/// </summary>
		protected readonly IList<MethodInfo> _definedGetters;

		/// <summary>
		/// Where the resources of this type are located.
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// Extension used for this resource type.
		/// </summary>
		public string Extension { get; set; }

		/// <summary>
		/// Creates an instance of the getter with specified getter flags.
		/// </summary>
		/// <param name="getterFlags">Flags used for getting getter methods.</param>
		public ResourceGetter(BindingFlags getterFlags = DefaultGetterFlags)
		{
			_definedGetters = GetType()
				.GetMethods(getterFlags)
				.Where(method => IsAGetter(method) && ShouldGetterBeIncluded(method))
				.ToList();
		}

		/// <summary>
		/// Creates an instance of the getter with specified getter flags and location.
		/// </summary>
		/// <param name="location">Location where the resources reside.</param>
		/// <param name="getterFlags">Flags used for getting getter methods.</param>
		public ResourceGetter(string location, BindingFlags getterFlags = DefaultGetterFlags) 
			: this(location, null, getterFlags) { }

		/// <summary>
		/// Creates an instance of the getter with specified getter flags, location and extension.
		/// </summary>
		/// <param name="location">Location where the resources reside.</param>
		/// <param name="extension">Extension used for the resources.</param>
		/// <param name="getterFlags">Flags used for getting getter methods.</param>
		public ResourceGetter(string location, string extension, BindingFlags getterFlags = DefaultGetterFlags) : this(getterFlags)
		{
			Location = location;
			Extension = extension;
		}

		/// <summary>
		/// Gets resource using one of the predefined getters
		/// that matches the given parameter list.
		/// </summary>
		/// <param name="parameters">Parameters for a predefined getter method.</param>
		/// <returns>Result of the getter method or null if one isn't found.</returns>
		public object Get(params object[] parameters)
		{
			var getter = GetMatchingGetter(parameters);

			return getter?.Invoke(this, parameters);
		}

		/// <summary>
		/// Gets resource using one of the predefined getters
		/// that matches the given parameter list.
		/// </summary>
		/// <typeparam name="T">Type of resource that is fetched.</typeparam>
		/// <param name="parameters">Parameters for a predefined getter method.</param>
		/// <returns>Result of the getter method or null if one isn't found.</returns>
		public T Get<T>(params object[] parameters)
		{
			var getter = GetMatchingGetter(parameters, type => type == typeof(T));

			return (T)getter?.Invoke(this, parameters) ?? default;
		}

		/// <summary>
		/// Gets a getter from <see cref="_definedGetters"/> that corresponds to the
		/// given <paramref name="parameters"/> and, if <paramref name="returnTypeMatches"/>
		/// is given, whose return type matches.
		/// </summary>
		/// <param name="parameters">Parameters for the getter.</param>
		/// <param name="returnTypeMatches">Method that defines if the return type
		/// of the getter is the target one.</param>
		/// <returns></returns>
		protected MethodInfo GetMatchingGetter(object[] parameters, Func<Type, bool> returnTypeMatches = null)
		{
			var paramsCount = parameters?.Count() ?? 0;

			foreach (var method in _definedGetters)
			{
				var methodParams = method.GetParameters();

				if (methodParams.Length == paramsCount)
				{
					var paramTypesMatch = true;

					for (int i = 0; i < methodParams.Length; i++)
					{
						var passedInParameterType = parameters[i]?.GetType();
						var requiredParameterType = methodParams[i].ParameterType;

						if
						(
							(passedInParameterType == null && requiredParameterType.IsValueType) ||
							(passedInParameterType != null && passedInParameterType != requiredParameterType)
						)
						{
							paramTypesMatch = false;
							break;
						}
					}

					if (paramTypesMatch && returnTypeMatches?.Invoke(method.ReturnType) != false)
					{
						return method;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Used for determining if the getter should be included (it shouldn't if
		/// it is one of predefined methods that start with <see cref="GetterMethodPrefix"/>).
		/// </summary>
		/// <param name="method">Getter method to check.</param>
		/// <returns>If the getter should be included or not.</returns>
		protected bool ShouldGetterBeIncluded(MethodInfo method)
		{
			var parameters = method.GetParameters();

			var shouldBeExcluded = method.Name switch
			{
				nameof(GetHashCode) => parameters.Length == 0 && method.ReturnType == typeof(int),
				nameof(GetType) => parameters.Length == 0 && method.ReturnType == typeof(Type),
				nameof(GetMatchingGetter)
					=> parameters.Length == 2 &&
						parameters[0].ParameterType == typeof(object[]) &&
						parameters[1].ParameterType == typeof(Func<Type, bool>) &&
						method.ReturnType == typeof(MethodInfo),
				_ => false,
			};

			return !shouldBeExcluded;
		}

		/// <summary>
		/// Used for determining if the given method is a getter.
		/// </summary>
		/// <param name="method">Method to test if it is a getter.</param>
		/// <returns>If the method is a getter or not.</returns>
		protected bool IsAGetter(MethodInfo method)
			=> method.ReturnType != typeof(void) &&
				method.Name.StartsWith(GetterMethodPrefix) &&
				method.Name != GetterMethodPrefix;

		/// <inheritdoc/>
		public override string ToString()
			=> _definedGetters?.ToString();
	}
}
