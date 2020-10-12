using Braco.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Braco.Services
{
	/// <summary>
	/// Used for setting up <see cref="WindowsProcessStarter"/> as <see cref="IProcessStarter"/>.
	/// </summary>
	public class WindowsProcessStarterSetup : ISetupService
	{
		/// <summary>
		/// Name of the section used for <see cref="ConfigurationSection"/>.
		/// </summary>
		public const string SectionName = "ProcessStarter";

		/// <summary>
		/// Key for the default terminate after configuration value inside current section.
		/// </summary>
		public const string DefaultTerminateAfterKey = "DefaultTerminateAfter";
		/// <summary>
		/// Key for the default use shell execute configuration value inside current section.
		/// </summary>
		public const string DefaultUseShellExecuteKey = "DefaultUseShellExecute";

		/// <inheritdoc/>
		public string ConfigurationSection { get; } = SectionName;

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			services.AddSingleton<IProcessStarter>(provider =>
			{
#pragma warning disable IDE0075 // Simplify conditional expression
				var defaultTerminateAfter = bool.TryParse(section[DefaultTerminateAfterKey], out var terminateAfter) ? terminateAfter : true;
				var defaultUseShellExecute = bool.TryParse(section[DefaultUseShellExecuteKey], out var useShellExecute) ? useShellExecute : true;
#pragma warning restore IDE0075 // Simplify conditional expression

				return new WindowsProcessStarter(provider.GetService<IPathManager>(), defaultTerminateAfter, defaultUseShellExecute);
			});
		}
	}
}
