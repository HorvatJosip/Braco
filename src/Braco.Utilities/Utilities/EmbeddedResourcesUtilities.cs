using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Braco.Utilities
{
	/// <summary>
	/// Utilities for working with embedded resources.
	/// <para>Example of an embedded resource would be a text file whose properties you've opened (right click on
	/// the file -> Properties) and selected "Embedded resource" as "Build Action".</para>
	/// <para>The utility methods will allow you to find the resource by providing only part of the resource name
	/// (for example, name of the file).</para>
	/// </summary>
	public static class EmbeddedResourcesUtilities
	{
		/// <summary>
		/// Separator for the location.
		/// </summary>
		public const string Separator = ".";

		/// <summary>
		/// Reads an embedded resource if found in one of the given assemblies.
		/// </summary>
		/// <param name="resourcePart">Part of the name of the resource to find.</param>
		/// <param name="assemblies">Assemblies in which to look for the embedded resource.</param>
		/// <returns>Contents of the resource at given location.</returns>
		public static async Task<string> ReadAsync(string resourcePart, params Assembly[] assemblies)
		{
			foreach (var assembly in assemblies)
			{
				// Try to find the name in the resources of the current assembly
				var name = assembly.GetManifestResourceNames().FirstOrDefault(name => name.Contains(resourcePart, StringComparison.InvariantCultureIgnoreCase));

				// If it was found...
				if(name != null)
				{
					// Open the stream
					using var stream = assembly.GetManifestResourceStream(name);

					// Open it using a stream reader
					using var reader = new StreamReader(stream);

					// Read its contents and return them
					return await reader.ReadToEndAsync();
				}
			}

			return null;
		}

		/// <summary>
		/// Reads an embedded resource if found in the current app domain.
		/// </summary>
		/// <param name="resourcePart">Part of the name of the resource to find.</param>
		/// <returns>Contents of the resource at given location.</returns>
		public static Task<string> ReadAsync(string resourcePart)
			=> ReadAsync(resourcePart, AppDomain.CurrentDomain.GetAssemblies());
	}
}
