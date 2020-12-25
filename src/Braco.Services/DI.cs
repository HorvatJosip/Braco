using AutoMapper;
using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Braco.Services
{
    /// <summary>
    /// Shortcuts for fetching injected services.
    /// </summary>
    public static class DI
    {
        /// <summary>
        /// Gets the configuration defined in the project.
        /// </summary>
        public static IConfigurationService Configuration => Get<IConfigurationService>();

        /// <summary>
        /// Gets the read-only configuration defined in the project.
        /// </summary>
        public static IConfiguration ReadOnlyConfiguration => Get<IConfiguration>();

        /// <summary>
        /// Gets the mapper used for this project.
        /// </summary>
        public static IMapper Mapper => Get<IMapper>();

        /// <summary>
        /// Gets the localizer used for this project.
        /// </summary>
        public static ILocalizer Localizer => Get<ILocalizer>();

        /// <summary>
        /// Paths that are used commonly throughout the application.
        /// </summary>
        public static IFileManager Paths => Get<IFileManager>();

		/// <summary>
		/// Resources that are used for the project.
		/// </summary>
		public static IResourceManager Resources => Get<IResourceManager>();

		/// <summary>
		/// Provider used for fetching services.
		/// </summary>
		public static IServiceProvider Provider { get; set; }

        /// <summary>
        /// Gets a service of the specified type using the <see cref="Provider"/>.
        /// </summary>
        /// <typeparam name="T">Type of service to get.</typeparam>
        public static T Get<T>()
            => Provider == null ? default : Provider.GetService<T>();
    }
}
