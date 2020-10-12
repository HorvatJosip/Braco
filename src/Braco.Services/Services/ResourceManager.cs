using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Braco.Services
{
	/// <summary>
	/// Default implementation of <see cref="IResourceManager"/>.
	/// </summary>
	public class ResourceManager : IResourceManager
	{
		private readonly IDictionary<Type, ResourceGetter> _resources = new Dictionary<Type, ResourceGetter>();

		/// <summary>
		/// Default separator used for <see cref="LocationSeparator"/>.
		/// </summary>
		public const string DefaultLocationSeparator = "/";

		/// <summary>
		/// Default location where the resources are located.
		/// </summary>
		public const string DefaultLocation = "Resources";

		/// <inheritdoc/>
		public ResourceGetter this[Type resourceType]
			=> _resources.TryGetValue(resourceType, out var resources) ? resources : null;

		/// <inheritdoc/>
		public string Location { get; set; }

		/// <inheritdoc/>
		public string LocationSeparator { get; set; }

		/// <summary>
		/// Creates an instance of the manager.
		/// </summary>
		public ResourceManager() { }

		/// <summary>
		/// Creates an instance of the manager with location specified.
		/// </summary>
		/// <param name="location">Location where the resources will be held.</param>
		public ResourceManager(string location) : this(location, DefaultLocationSeparator) { }

		/// <summary>
		/// Creates an instance of the manager with location and separator specified.
		/// </summary>
		/// <param name="location">Location where the resources will be held.</param>
		/// <param name="locationSeparator">Separator used for separating multiple resources.</param>
		public ResourceManager(string location, string locationSeparator)
		{
			Location = location;
			LocationSeparator = locationSeparator;
		}

		/// <inheritdoc/>
		public bool Add<TResourceGetter>(TResourceGetter instance = null) where TResourceGetter : ResourceGetter
		{
			var type = typeof(TResourceGetter);

			if (_resources.ContainsKey(type)) return false;

			Set(instance);
			return true;
		}

		/// <inheritdoc/>
		public IList<Type> AddFromAppDomain(AppDomain appDomain)
			=> appDomain?.GetAssemblies().SelectMany(ass => AddFromAssembly(ass)).ToList();

		/// <inheritdoc/>
		public IList<Type> AddFromAssembly(Assembly assembly)
		{
			var result = new List<Type>();

			assembly?.GetTypes().ForEach(type =>
			{
				if (Set(type))
				{
					result.Add(type);
				}
			});

			return result;
		}

		/// <inheritdoc/>
		public bool[] AddRange(params object[] instances)
			=> instances
				.Select(instance =>
				{
					var type = instance?.GetType();

					if (type == null) return false;

					return 
						typeof(ResourceGetter).IsAssignableFrom(type) &&
						Set(type, instance);
				})
				.ToArray();

		/// <inheritdoc/>
		public bool[] AddRange(params Type[] resourceGetterTypes)
			=> resourceGetterTypes.Select(type => Set(type)).ToArray();

		/// <inheritdoc/>
		public object Get<TResourceGetter>(params object[] parameters) where TResourceGetter : ResourceGetter
		{
			if (_resources.TryGetValue(typeof(TResourceGetter), out var resources))
			{
				return resources.Get(parameters);
			}

			return null;
		}

		/// <inheritdoc/>
		public TResourceType Get<TResourceGetter, TResourceType>(params object[] parameters) where TResourceGetter : ResourceGetter
		{
			if (_resources.TryGetValue(typeof(TResourceGetter), out var resources))
			{
				return resources.Get<TResourceType>(parameters);
			}

			return default;
		}

		/// <inheritdoc/>
		public void Set<TResourceGetter>(TResourceGetter instance = null) where TResourceGetter : ResourceGetter
		{
			var getter = instance ?? Activator.CreateInstance<TResourceGetter>();

			if (Location.IsNotNullOrWhiteSpace() && getter.Location.IsNotNullOrWhiteSpace())
			{
				getter.Location = string.Join(LocationSeparator, Location, getter.Location);
			}

			_resources[typeof(TResourceGetter)] = getter;
		}

		/// <inheritdoc/>
		public bool Set(Type resourceGetterType)
			=> Set(resourceGetterType, null);

		/// <inheritdoc/>
		public bool Set(Type resourceGetterType, object instance)
		{
			if
			(
				!typeof(ResourceGetter).IsAssignableFrom(resourceGetterType) ||
				_resources.ContainsKey(resourceGetterType)
			)
			{
				return false;
			}

			var getter = (ResourceGetter)(instance ?? Activator.CreateInstance(resourceGetterType));

			if (Location.IsNotNullOrWhiteSpace() && getter.Location.IsNotNullOrWhiteSpace())
			{
				getter.Location = string.Join(LocationSeparator, Location, getter.Location);
			}

			_resources[resourceGetterType] = getter;
			return true;
		}
	}
}
