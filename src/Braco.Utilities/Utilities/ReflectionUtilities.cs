using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Braco.Utilities
{
	/// <summary>
	/// Utilities for performing reflection.
	/// </summary>
	public static class ReflectionUtilities
    {
		/// <summary>
		/// Name of the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
		/// </summary>
        public const string PropChangedName = nameof(INotifyPropertyChanged.PropertyChanged);

        /// <summary>
        /// Subscribes to <see cref="INotifyPropertyChanged.PropertyChanged"/> event
        /// (if it exists) on the target object and calls the given method if the
        /// changed property name matches the given property name.
        /// </summary>
        /// <param name="target">Object to subscribe to for changes.</param>
        /// <param name="property">Property to track for changes on the target object.</param>
        /// <param name="onPropertyChanged">Method to invoke when the given property on the target object changes.</param>
        public static bool ListenForPropertyChanges(object target, string property, Action<object> onPropertyChanged)
            => ListenForPropertyChanges(target, (sender, propName) =>
            {
                // If the target property has changed...
                if (propName == property)
                    // Invoke the on changed
                    onPropertyChanged?.Invoke(sender);
            });

        /// <summary>
        /// Subscribes to <see cref="INotifyPropertyChanged.PropertyChanged"/> event
        /// (if it exists) on the target object.
        /// </summary>
        /// <param name="target">Object to subscribe to for changes.</param>
        /// <param name="onPropertyChanged">Method to invoke when a property on the target object changes.</param>
        public static bool ListenForPropertyChanges(object target, Action<object, string> onPropertyChanged)
        {
            // Get the property changed event
            var propChangedEvent = target?.GetType().GetEvent(PropChangedName);

            // If it doesn't exist...
            if (propChangedEvent == null)
                // Bail
                return false;

            // Subscribe to its property changed event
            propChangedEvent.AddEventHandler
            (
                target: target,
                handler: new PropertyChangedEventHandler((sender, e) => onPropertyChanged?.Invoke(sender, e.PropertyName))
            );

            return true;
        }

        /// <summary>
        /// Raises <see cref="INotifyPropertyChanged.PropertyChanged"/> event
        /// on the given object for the given property.
        /// <para>Note: requires PropertyChanged.Fody injection using
		/// <see cref="PropertyChanged.AddINotifyPropertyChangedInterfaceAttribute"/>.</para>
        /// </summary>
        /// <param name="target">Object that contains the event.</param>
        /// <param name="property">Property for which we are raising the event.</param>
		/// <returns>If it was invoked or not.</returns>
        public static bool RaisePropertyChanged(object target, string property)
        {
            // Get the type from the target object
            var type = target?.GetType();

            // Try to find the given property
            var propertyInfo = type?.GetProperty(property);

            // If it wasn't found...
            if (propertyInfo == null)
                // Bail
                return false;

            // Get the OnPropertyChanged method
            var onPropertyChanged = type.GetAMethod(typeof(PropertyChangedEventArgs));

            // If it isn't defined...
            if (onPropertyChanged == null)
                // Bail
                return false;

            // Invoke the method with appropriate arguments
            onPropertyChanged.Invoke(target, new object[] { new PropertyChangedEventArgs(property) });

            return true;
        }

        /// <summary>
        /// Tries to find type by name in the current <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="typeName">Name of the type to find (can be full or assembly qualified name as well).</param>
        /// <returns>Type found using the given name (or null if not found).</returns>
        public static Type FindType(string typeName)
        {
            if (typeName == null) throw new ArgumentNullException(nameof(typeName));

			// Go through all of the assemblies loaded in the current domain
			foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				// Try to find the target type in it by name
				var targetType = assembly.GetTypes().FirstOrDefault(type =>
					typeName.In(type.Name, type.FullName, type.AssemblyQualifiedName)
				);

				// If it exists...
				if (targetType != null)
					// Return it
					return targetType;
			}

            // Type wasn't found, return null
            return null;
        }

		/// <summary>
		/// Finds all of the types that are assignable from <paramref name="baseType"/>
		/// in the current app domain.
		/// </summary>
		/// <param name="baseType">Type which the types need to be assignable from.</param>
		/// <returns>Types that are assignable from <paramref name="baseType"/>.</returns>
		public static List<Type> FindAssignableTypes(Type baseType)
			=> FindAssignableTypes(baseType, AppDomain.CurrentDomain.GetAssemblies());

		/// <summary>
		/// Finds all of the types that are assignable from <paramref name="baseType"/>
		/// in the given <paramref name="assemblies"/>.
		/// </summary>
		/// <param name="baseType">Type which the types need to be assignable from.</param>
		/// <param name="assemblies">Assemblies to search through.</param>
		/// <returns>Types that are assignable from <paramref name="baseType"/>.</returns>
		public static List<Type> FindAssignableTypes(Type baseType, params Assembly[] assemblies)
		{
			// Initialize the list
			var types = new List<Type>();

			// Go through all of the assemblies loaded in the current domain
			foreach (var assembly in assemblies)
			{
				// Go through all of the types defined in the current assembly
				foreach (var type in assembly.GetTypes())
				{
					// If the base type is assignable from current type...
					if (baseType.IsAssignableFrom(type))
					{
						// Add it to the list
						types.Add(type);
					}
				}
			}
			
			// Return the list
			return types;
		}

		/// <summary>
		/// Used for registering a specific <see cref="TypeConverter"/>.
		/// <para>Common usage would be registering a converter for already defined type such as
		/// a collection of specific type. </para>
		/// <para>Example: you want to have <see cref="List{T}"/> be loaded
		/// using <see cref="SettingAttribute"/>. For <typeparamref name="T"/>, you want to
		/// provide the list (<see cref="List{T}"/>) and for <typeparamref name="TConverter"/>, you
		/// want to provide <see cref="TypeConverter"/> that is used for converting <see cref="List{T}"/>.</para>
		/// </summary>
		/// <typeparam name="T">Type which is being converter using <typeparamref name="TConverter"/>.</typeparam>
		/// <typeparam name="TConverter"><see cref="TypeConverter"/> for <typeparamref name="T"/>.</typeparam>
		/// <returns>The newly created <see cref="TypeDescriptionProvider"/> that was used
		/// to add the specified attributes.</returns>
		public static TypeDescriptionProvider RegisterConverter<T, TConverter>()
			where TConverter : TypeConverter
			=> TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TConverter)));
    }
}
