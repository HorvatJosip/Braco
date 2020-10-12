using Microsoft.Extensions.Configuration;
using System;

namespace Braco.Services
{
	/// <summary>
	/// Defines that the inheriting member will initialize after
	/// the <see cref="IServiceProvider"/> has been setup.
	/// </summary>
	public interface IInitializeAfterBuildingServices : IHaveConfigurationSection
	{
		/// <summary>
		/// Name of the section where configuration for members that inherit
		/// <see cref="IInitializeAfterBuildingServices"/> should be placed.
		/// </summary>
		public const string InitializersSection = "Initializers";

		/// <summary>
		/// Method used for initializing after the service provider
		/// has been built.
		/// </summary>
		/// <param name="provider">Service provider for this project.</param>
		/// <param name="configuration">Read-only configuration.</param>
		/// <param name="section">Section in the configuration dedicated to this <see cref="IInitializeAfterBuildingServices"/>.</param>
		void Initialize(IServiceProvider provider, IConfiguration configuration, IConfigurationSection section);
	}
}
