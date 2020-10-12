using Braco.Services;
using System.Reflection;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Utilties for WPF pack.
	/// </summary>
    public static class PackUtilities
    {
        /// <summary>
        /// Constant that defines root uri of a WPF application.
        /// </summary>
        public const string PackUri = "pack://application:,,,/";

        /// <summary>
        /// Gets root pack uri of the given assembly (which should be a WPF app).
        /// </summary>
		/// <param name="assemblyName">Name of the assembly for which to get the
		/// root pack uri.</param>
		/// <returns>Root pack uri from given assembly.</returns>
        public static string GetRootPackUri(string assemblyName)
            => $"{PackUri}{assemblyName};component/";

		/// <summary>
		/// Gets root pack uri of the assembly registered in <see cref="AssemblyGetter"/>.
		/// </summary>
		/// <returns>Root pack uri with assembly from <see cref="AssemblyGetter"/>.</returns>
		public static string GetRootPackUri()
			=> GetRootPackUri(DI.Resources.Get<AssemblyGetter, string>());

		/// <summary>
		/// Gets root pack uri of the calling assembly (which should be a WPF app).
		/// </summary>
		/// <returns>Root pack uri of the calling assembly.</returns>
		public static string GetRootPackUriForCallingAssembly()
			=> GetRootPackUri(Assembly.GetCallingAssembly().GetName().Name);

		/// <summary>
		/// Appends the given string to the root pack uri.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly for which to get the
		/// root pack uri.</param>
		/// <param name="suffix">String to append to root pack uri.</param>
		/// <returns>Root pack uri with <paramref name="suffix"/> appended at the end.</returns>
		public static string GetRootPackUriWithSuffix(string assemblyName, string suffix)
			=> $"{GetRootPackUri(assemblyName)}{suffix}";

		/// <summary>
		/// Appends the given string to the root pack uri that is obtained
		/// using the <see cref="AssemblyGetter"/>.
		/// </summary>
		/// <param name="suffix">String to append to root pack uri.</param>
		/// <returns>Root pack uri with <paramref name="suffix"/> appended at the end.</returns>
		public static string GetRootPackUriWithSuffix(string suffix)
			=> $"{GetRootPackUri()}{suffix}";

		/// <summary>
		/// Appends the given string to the root pack uri that is obtained
		/// using the <see cref="Assembly.GetCallingAssembly"/> method.
		/// </summary>
		/// <param name="suffix">String to append to root pack uri.</param>
		/// <returns>Root pack uri with <paramref name="suffix"/> appended at the end.</returns>
		public static string GetRootPackUriForCallingAssemblyWithSuffix(string suffix)
			=> $"{GetRootPackUriForCallingAssembly()}{suffix}";
	}
}
