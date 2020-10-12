namespace Braco.Services
{
	/// <summary>
	/// Defines that the inheriting member has a dedicated
	/// section inside the configuration.
	/// </summary>
	public interface IHaveConfigurationSection
	{
		/// <summary>
		/// Name of the section inside the configuration.
		/// </summary>
		string ConfigurationSection { get; }
	}
}
