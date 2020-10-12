using Braco.Services.Media.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using VideoLibrary;

namespace Braco.Services.Media
{
	/// <summary>
	/// Sets up the service implementations for the specific abstractions:
	/// <para><see cref="MP3AudioSplitter"/> for <see cref="IMediaSplitter"/>.</para>
	/// <para><see cref="YouTubeAudioDownloader"/> for <see cref="IMediaDownloader"/>.</para>
	/// <para><see cref="YouTubeVideoDownloader"/> for itself.</para>
	/// <para><see cref="YouTubeAudioDownloader"/> for itself.</para>
	/// <para>Note: adding to the service collection for the same abstraction twice will make
	/// the second call for adding the service override the first one.</para>
	/// </summary>
	public class ServicesSetup : ISetupService
	{
		/// <summary>
		/// Not used.
		/// </summary>
		public string ConfigurationSection { get; }

		/// <inheritdoc/>
		public void Setup(IServiceCollection services, IConfiguration configuration, IConfigurationSection section)
		{
			var videoDownloader = new YouTubeVideoDownloader(YouTube.Default);
			var audioDownloader = new YouTubeAudioDownloader(videoDownloader);

			services.AddSingleton(audioDownloader);
			services.AddSingleton(videoDownloader);
			services.AddSingleton<IMediaDownloader>(audioDownloader);
			services.AddSingleton<IMediaSplitter>(new MP3AudioSplitter());
		}
	}
}
