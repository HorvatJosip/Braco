using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Braco.Services
{
	/// <summary>
	/// Default setup for <see cref="IMapper"/>. Simply adds
	/// a mapper to the service collection.
	/// </summary>
	public class MapperSetup : ISetupService
	{
		/// <summary>
		/// Not used.
		/// </summary>
		public string ConfigurationSection { get; }

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			var mapperConfiguration = new MapperConfiguration(config =>
			{
				config.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
			});

			var mapper = mapperConfiguration.CreateMapper();

			services.AddSingleton(mapper);
		}
	}
}
