using Braco.Services.Media.Abstractions;
using Braco.Utilities;
using Braco.Utilities.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoLibrary;

namespace Braco.Services.Media
{
	/// <summary>
	/// Implementation of <see cref="IMediaDownloader"/> that works with YouTube videos.
	/// </summary>
	public class YouTubeVideoDownloader : IMediaDownloader
	{
		private readonly YouTube _youTube;

		/// <summary>
		/// Creates the downloader.
		/// </summary>
		/// <param name="youTube"><see cref="YouTube"/> instance (You can use <see cref="YouTube.Default"/>).</param>
		public YouTubeVideoDownloader(YouTube youTube)
		{
			_youTube = youTube ?? throw new ArgumentNullException(nameof(youTube));
		}

		/// <inheritdoc/>
		public async Task<MediaDownloadResponse> DownloadAsync(MediaDownloadRequest request, CancellationToken cancellationToken)
		{
			if (!IsUriInValidFormat(request.DownloadInfo.Uri, out var uri))
				return MediaDownloadResponse.FromErrors(UnfinishedReasons.InvalidYouTubeUri, "YouTube URI is invalid.");

			YouTubeVideo videoWithBestBitRate;

			try
			{
				// Get all videos with given uri
				var videos = await _youTube.GetAllVideosAsync(uri);

				// Find one with the best bitrate
				videoWithBestBitRate = videos?.OrderByDescending(video => video.AudioBitrate).FirstOrDefault();

				// If it wasn't found...
				if (videoWithBestBitRate == null)
					// Bail
					return MediaDownloadResponse.FromErrors(UnfinishedReasons.VideoNotFound, $"Couldn't fetch the video from {uri}...");

				// If there was a request to cancel, bail
				if (cancellationToken.IsCancellationRequested) return MediaDownloadResponse.Cancelled();

				// Make sure that the given directory exist
				request.Directory.Create();

				// Make sure to remove invalid file name characters
				var cleanTitle = PathUtilities.GetFileNameWithoutInvalidChars(videoWithBestBitRate.Title);
				
				// Construct video destination
				var videoPath = Path.Combine(request.Directory.FullName, $"{cleanTitle}{PathUtilities.GetExtensionWithDot(videoWithBestBitRate.FileExtension)}");

				// Create data about the video
				var videoData = new MediaData { Title = cleanTitle };

				// Inform about the title of the video
				request.DataCallback?.Invoke(videoData);

				// Get the video bytes
				var videoBytes = await videoWithBestBitRate.GetBytesAsync();

				try
				{
					// Get a file for writing
					using var file = File.OpenWrite(videoPath);

					// Write the data to file in chunks
					var writeResult = await file.WriteInChunksAsync(videoBytes, request.DownloadInfo.ChunkSize, cancellationToken);

					// If it was written, return the result, otherwise, just return null
					return writeResult ? new MediaDownloadResponse
					{
						Data = videoData,
						File = new FileInfo(videoPath)
					} : MediaDownloadResponse.Cancelled();
				}
				catch (Exception ex)
				{
					return MediaDownloadResponse.FromErrors(UnfinishedReasons.VideoFileWritingError, $"{ex}");
				}
			}
			catch(Exception ex)
			{
				return MediaDownloadResponse.FromErrors(UnfinishedReasons.VideoDownloadError, $"{ex}");
			}
		}

		/// <inheritdoc/>
		public bool IsUriInValidFormat(string uri, out string formatted)
			=> YouTubeUriValidator.IsUriInValidFormat(uri, out formatted);
	}
}
