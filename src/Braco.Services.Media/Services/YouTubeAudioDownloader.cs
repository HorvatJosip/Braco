using Braco.Services.Abstractions;
using Braco.Services.Media.Abstractions;
using MediaToolkit;
using MediaToolkit.Model;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Braco.Services.Media
{
	/// <summary>
	/// Used for extracting audio from a YouTube video.
	/// </summary>
	public class YouTubeAudioDownloader : IMediaDownloader
	{
		private readonly YouTubeVideoDownloader _youTubeVideoDownloader;

		/// <summary>
		/// Creates an instance of the downloader with required YouTube video downloader.
		/// </summary>
		/// <param name="youTubeVideoDownloader">YouTube video downloader.</param>
		public YouTubeAudioDownloader(YouTubeVideoDownloader youTubeVideoDownloader)
		{
			_youTubeVideoDownloader = youTubeVideoDownloader ?? throw new ArgumentNullException(nameof(youTubeVideoDownloader));
		}

		/// <inheritdoc/>
		public async Task<MediaDownloadResponse> DownloadAsync(MediaDownloadRequest request, CancellationToken cancellationToken)
		{
			try
			{
				// Extract where to download audio
				var audioLocation = request.Directory;

				// Make sure to download the video to correct directory
				request.Directory = new DirectoryInfo(DI.Configuration[ConfigurationKeys.VideoDownloadDirectory]);

				// Download the video first
				var videoDownloadResult = await _youTubeVideoDownloader.DownloadAsync(request, cancellationToken);

				// If it didn't finish, bail
				if (!videoDownloadResult.Finished) return videoDownloadResult;

				// Invoke the callback in order to signal that the download finished
				request.DataCallback?.Invoke(videoDownloadResult.Data);

				// Make sure that the given directory exists
				audioLocation.Create();
				
				// Construct audio destination
				var audioPath = Path.Combine(audioLocation.FullName, $"{videoDownloadResult.Data.Title}{MP3AudioSplitter.Mp3Extension}");

				try
				{
					// Create conversion engine
					using var engine = new Engine();

					// Define input and output files for the engine
					var inputFile = new MediaFile(videoDownloadResult.File.FullName);
					var outputFile = new MediaFile(audioPath);

					// Read metadata from the input file
					engine.GetMetadata(inputFile);

					// If there was a cancellation request, bail
					if (cancellationToken.IsCancellationRequested) return null;

					// Convert the video into audio file
					await Task.Run(() => engine.Convert(inputFile, outputFile));
				}
				catch(Exception ex)
				{
					return MediaDownloadResponse.FromErrors(UnfinishedReasons.VideoToAudioConversionError, $"{ex}");
				}

				// If there was a cancellation request...
				if (cancellationToken.IsCancellationRequested)
				{
					// Delete the file
					File.Delete(audioPath);

					// Bail
					return MediaDownloadResponse.Cancelled();
				}

				// Create the response
				var response = new MediaDownloadResponse 
				{
					Messages = new List<Message>(),
					Data = videoDownloadResult.Data,
					File = new FileInfo(audioPath)
				};

				try
				{
					// Open the MP3 file...
					using var mp3FileReader = new Mp3FileReader(audioPath);

					// Inform about the duration
					response.Data.Duration = mp3FileReader.TotalTime;
				}
				catch(Exception ex)
				{
					response.Messages.Add(Message.FromWarning($"There was an error trying to read audio duration: {ex}"));
				}

				// Return the response
				return response;
			}
			catch (Exception ex)
			{
				return MediaDownloadResponse.FromErrors(UnfinishedReasons.AudioDownloadError, $"{ex}");
			}
		}

		/// <inheritdoc/>
		public bool IsUriInValidFormat(string uri, out string formatted)
			=> YouTubeUriValidator.IsUriInValidFormat(uri, out formatted);
	}
}
