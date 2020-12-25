using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Braco.Utilities.Extensions
{
	/// <summary>
	/// Extensions for <see cref="Type"/> class.
	/// </summary>
    public static class TypeExtensions
    {
        #region Constants

        /// <summary>
        /// Default <see cref="BindingFlags"/> used for getting fields and properties.
        /// </summary>
        public const BindingFlags DefaultPrieldFlags =
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        /// Default <see cref="BindingFlags"/> used for getting methods.
        /// </summary>
        public const BindingFlags DefaultMethodFlags =
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static |
            BindingFlags.InvokeMethod | BindingFlags.Instance;

		/// <summary>
		/// Indicator that means a field is a backing field (if field name contains it).
		/// <para>Note: this might change in the future. This is compiler based. 
		/// So, if you use it, be careful upon compiling new version of the code.
		/// Test that the code that uses this is working properly.</para>
		/// </summary>
        public const string BackingFieldIndicator = "k__BackingField";

		/// <summary>
		/// Separator used for nested prield format.
		/// </summary>
        public const string NestedPrieldSeparator = ".";

		#endregion

		#region Methods

		/// <summary>
		/// Used to get all the fields and properties defined by a type.
		/// </summary>
		/// <param name="type">Type to get the fields and properties from.</param>
		/// <param name="skipBackingFields">If there are backing fields for the properties,
		/// should they be skipped or not?
		/// <para>Note: this should be used carefully (see <see cref="BackingFieldIndicator"/>).</para>
		/// </param>
		/// <param name="flags"><see cref="BindingFlags"/> used for accessing the fields and properties.</param>
		/// <returns>Collection of fields and properties.</returns>
		public static List<Prield> GetPrields(this Type type, bool skipBackingFields = true, BindingFlags flags = DefaultPrieldFlags)
        {
            if (type == null) throw new NullReferenceException();

            // Generate prields out of fields and properties
            var fields = type.GetFields(flags).Select(field => new Prield(field));
            var properties = type.GetProperties(flags).Select(field => new Prield(field));

            // If backing fields need to be skipped...
            if (skipBackingFields)
                // Remove backing fields from the fields collection
                fields = fields.Where(field => !field.Member.Name.Contains(BackingFieldIndicator)).ToList();

            // Return all of the prields
            return fields.Concat(properties).ToList();
        }

        /// <summary>
        /// Takes in an object and the property path separated by dots and extracts
        /// value from the ending property.
        /// </summary>
        /// <param name="type">Type from which to get the nested property.</param>
        /// <param name="target">Target object from which to extract the ending property value.</param>
        /// <param name="dotSeparatedNotation">Path to the property separated using <see cref="NestedPrieldSeparator"/>.
        /// <para>Example: "Person.Age"</para></param>
        /// <returns></returns>
        public static Prield GetNestedPrield(this Type type, object target, string dotSeparatedNotation)
        {
            var parts = dotSeparatedNotation.Split(new string[] { NestedPrieldSeparator }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
                return new Prield(type.GetProperty(dotSeparatedNotation));

            Prield prield = null;

            for (int i = 0; i < parts.Length; i++)
            {
                prield = new Prield(type.GetProperty(parts[i]));
                var currentValue = prield.GetValue(target);

                if (currentValue == null && i != parts.Length - 1)
                    prield.SetValue(target, Activator.CreateInstance(prield.Type));

                target = prield.GetValue(target);
                type = target?.GetType();
            }

            return prield;
        }

        /// <summary>
        /// Gets a value from a field or a property on a target object.
        /// </summary>
        /// <param name="type">Type that declares the field or property.</param>
        /// <param name="prieldName">Name of the field or property to get the value from.</param>
        /// <param name="target">Object to get the value from.</param>
        /// <param name="indexerParams">Parameters for the indexer (if the property is an indexer).</param>
        /// <returns>Value from the target object of the specified field or property</returns>
        public static object GetValue(this Type type, string prieldName, object target, object[] indexerParams = null)
        {
            if (type == null) throw new NullReferenceException();
            if (prieldName == null) throw new ArgumentNullException(nameof(prieldName));

            // Get the prields of the type
            var prields = type.GetPrields();

            // Find the one that matches the name in the parameters
            var targetPrield = prields.Find(prield => prield.Member.Name == prieldName);

            // If it wasn't found, return null, otherwise get value of the prield from the target
            return targetPrield?.GetValue(target, indexerParams);
        }

        /// <summary>
        /// Sets a value of a field or a property on a target object.
        /// </summary>
        /// <param name="type">Type that declares the field or property.</param>
        /// <param name="prieldName">Name of the field or property to get the value from.</param>
        /// <param name="target">Object to get the value from.</param>
        /// <param name="value">Value to set the prield to.</param>
        /// <param name="indexerParams">Parameters for the indexer (if the property is an indexer).</param>
        /// <returns>Value from the target object of the specified field or property</returns>
        public static void SetValue(this Type type, string prieldName, object target, object value, object[] indexerParams = null)
        {
            if (type == null) throw new NullReferenceException();
            if (prieldName == null) throw new ArgumentNullException(nameof(prieldName));

            // Get the prields of the type
            var prields = type.GetPrields();

            // Find the one that matches the name in the parameters
            var targetPrield = prields.Find(prield => prield.Member.Name == prieldName);

            // If it wasn't found, return null, otherwise get value of the prield from the target
            targetPrield?.SetValue(target, value, indexerParams);
        }

        /// <summary>
        /// Gets a method based on name and conditions for the parameters.
        /// </summary>
        /// <param name="type">Type to get the method from.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="conditions">Defines conditions based on the method and its parameters.</param>
        /// <returns>Method that matches the given name and conditions.</returns>
        /// <param name="flags"><see cref="BindingFlags"/> used for accessing the methods.</param>
        public static MethodInfo GetAMethod(this Type type, string name, Func<MethodInfo, ParameterInfo[], bool> conditions,
            BindingFlags flags = DefaultMethodFlags)
        {
            if (type == null) throw new NullReferenceException();
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));

            return type
                // Get all of the methods using the binding flags
                .GetMethods(flags)
                // Get all of the overloads by name
                .Where(method => method.Name == name)
                // Return the first one to match the method and parameter conditions
                .FirstOrDefault(method => conditions(method, method.GetParameters()));
        }

        /// <summary>
        /// Gets a method based on name and conditions for method and its parameters.
        /// </summary>
        /// <param name="type">Type to get the method from.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="parameterConditions">Defines conditions for parameters of the method.</param>
        /// <returns>Method that matches the given name and parameter conditions.</returns>
        /// <param name="flags"><see cref="BindingFlags"/> used for accessing the methods.</param>
        public static MethodInfo GetAMethod(this Type type, string name, Func<ParameterInfo[], bool> parameterConditions,
            BindingFlags flags = DefaultMethodFlags)
        {
            if (type == null) throw new NullReferenceException();
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (parameterConditions == null) throw new ArgumentNullException(nameof(parameterConditions));

            return GetAMethod(type, name, (_, parameters) => parameterConditions(parameters), flags);
        }

        /// <summary>
        /// Gets a method based on name and the types it takes in as parameters.
        /// </summary>
        /// <param name="type">Type to get the method from.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="parameterTypes">Types that the method takes in as parameters.</param>
        /// <returns>Method that matches the given name and parameter types.</returns>
        public static MethodInfo GetAMethod(this Type type, string name, params Type[] parameterTypes)
            => GetAMethod(type, name, parameters =>
            {
                // If the method should have no parameters...
                if (parameterTypes.IsNullOrEmpty())
                    // Return true if the method has no parameters, false otherwise
                    return parameters.Length == 0;

                // Get the parameter types from the current method
                var methodParameterTypes = parameters.Select(parameter => parameter.ParameterType);

                // Return whether or not all of the types match
                return Enumerable.SequenceEqual(parameterTypes, methodParameterTypes);
            });

        /// <summary>
        /// Gets a method based on conditions for it and its parameters.
        /// </summary>
        /// <param name="type">Type to get the method from.</param>
        /// <param name="conditions">Defines conditions based on the method and its parameters.</param>
        /// <returns>Method that matches the given conditions.</returns>
        /// <param name="flags"><see cref="BindingFlags"/> used for accessing the methods.</param>
        public static MethodInfo GetAMethod(this Type type, Func<MethodInfo, ParameterInfo[], bool> conditions,
            BindingFlags flags = DefaultMethodFlags)
        {
            if (type == null) throw new NullReferenceException();
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));

            return type
                // Get all of the methods using the binding flags
                .GetMethods(flags)
                // Return the first one to match the parameter conditions
                .FirstOrDefault(method => conditions(method, method.GetParameters()));
        }

        /// <summary>
        /// Gets a method based on conditions for the parameters.
        /// </summary>
        /// <param name="type">Type to get the method from.</param>
        /// <param name="parameterConditions">Defines conditions for parameters of the method.</param>
        /// <returns>Method that matches the given name and parameter conditions.</returns>
        /// <param name="flags"><see cref="BindingFlags"/> used for accessing the methods.</param>
        public static MethodInfo GetAMethod(this Type type, Func<ParameterInfo[], bool> parameterConditions,
            BindingFlags flags = DefaultMethodFlags)
        {
            if (type == null) throw new NullReferenceException();
            if (parameterConditions == null) throw new ArgumentNullException(nameof(parameterConditions));

            return GetAMethod(type, (_, parameters) => parameterConditions(parameters), flags);
        }

        /// <summary>
        /// Gets a method based on the types it takes in as parameters.
        /// </summary>
        /// <param name="type">Type to get the method from.</param>
        /// <param name="parameterTypes">Types that the method takes in as parameters.</param>
        /// <returns>Method that matches the given name and parameter types.</returns>
        public static MethodInfo GetAMethod(this Type type, params Type[] parameterTypes)
            => GetAMethod(type, parameters =>
            {
                // If the method should have no parameters...
                if (parameterTypes.IsNullOrEmpty())
                    // Return true if the method has no parameters, false otherwise
                    return parameters.Length == 0;

                // Get the parameter types from the current method
                var methodParameterTypes = parameters.Select(parameter => parameter.ParameterType);

                // Return whether or not all of the types match
                return Enumerable.SequenceEqual(parameterTypes, methodParameterTypes);
            });

        /// <summary>
        /// Gets collection information about a type.
        /// </summary>
        /// <param name="type">Type to test if it is a collection.</param>
        /// <returns>Instance of <see cref="CollectionInfo"/> about the <paramref name="type"/>.</returns>
        public static CollectionInfo GetCollectionInfo(this Type type)
        {
            if (type == null) throw new NullReferenceException();

            // Return collection information about the type
            return new CollectionInfo(type);
        }

        #endregion
    }
}
