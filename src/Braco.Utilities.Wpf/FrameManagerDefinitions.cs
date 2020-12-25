using Braco.Services;
using Braco.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Helpers used for specifying <see cref="IFrameManager"/>s for different pages
	/// from JSON (<see cref="DI.ReadOnlyConfiguration"/>).
	/// </summary>
	public static class FrameManagerDefinitions
	{
		/// <summary>
		/// Name of the section used for defining the <see cref="IFrameManager"/>s for
		/// different pages.
		/// <para>Withing this section, you should specify different windows as keys.</para>
		/// <para>For each window, you should specify keys for each page that defines a frame.</para>
		/// <para>For each page that has a frame, you should specify an array of pages that can be shown
		/// withing that frame.</para>
		/// <example>
		/// Example of the section:
		/// <code>
		/// 	"Frames": {
		///			"MainWindowViewModel": {
		///				"ProjectHomePageViewModel": [
		///					"ConstantsPageViewModel",
		///					"FontsPageViewModel",
		///					"ImagesPageViewModel",
		///					"LocalizationPageViewModel",
		///					"PagesPageViewModel"
		///				]
		///			}
		///		}
		/// </code>
		/// </example>
		/// </summary>
		public const string JsonSectionName = "Frames";

		private static readonly MethodInfo _getFrameManagerMethod =
			typeof(IWindow).GetAMethod(nameof(IWindow.GetFrameManager), typeof(int?));

		private static readonly Dictionary<Type, IFrameManager> _cache = new();
		private static readonly List<FrameManagerDefinition> _definitions = new();

		/// <summary>
		/// Sets up frame manager definitions from given <paramref name="configuration"/>.
		/// </summary>
		/// <param name="configuration">Configuration from which to extract the definitions.</param>
		public static void InitializeFromConfiguration(IConfiguration configuration)
		{
			var frames = configuration.GetSection(JsonSectionName);

			foreach (var window in frames.GetChildren())
			{
				var windowType = ReflectionUtilities.FindType(window.Key);

				if (windowType == null) continue;

				foreach (var frameHolder in window.GetChildren())
				{
					var frameHolderType = ReflectionUtilities.FindType(frameHolder.Key);

					if (frameHolderType == null) continue;

					var children = new List<Type> { frameHolderType };

					foreach (var frameChild in frameHolder.GetChildren())
					{
						var childType = ReflectionUtilities.FindType(frameChild.Value);

						if (childType == null) continue;

						children.Add(childType);
					}

					_definitions.Add(new FrameManagerDefinition
					(
						Window: windowType,
						FramePage: frameHolderType,
						FrameChildren: children
					));
				}
			}
		}

		/// <summary>
		/// Gets frame manager defined for given <paramref name="page"/>.
		/// </summary>
		/// <param name="page">Page for which to get the frame manager.</param>
		/// <returns>Frame manager used for <paramref name="page"/>.</returns>
		public static IFrameManager Get(Type page)
		{
			if (_cache.TryGetValue(page, out var manager)) return manager;

			var windowsManager = DI.Get<IWindowsManager>();

			var definitions = _definitions.FirstOrDefault(def => def.FrameChildren.Any(child => child == page));

			IFrameManager result = null;

			if (definitions != null)
			{
				var window = windowsManager[definitions.Window];

				if 
				(
					window != null &&
					_getFrameManagerMethod.MakeGenericMethod(definitions.FramePage)
						.Invoke(window, new object[] { 0 }) is IFrameManager frameManager
				)
				{
					result = frameManager;
				}
			}

			result ??= windowsManager.ActiveWindow.InitialFrameManager;

			if (!_cache.ContainsKey(page))
			{
				_cache.Add(page, result);
			}

			return result;
		}

		record FrameManagerDefinition(Type Window, Type FramePage, List<Type> FrameChildren);
	}
}
