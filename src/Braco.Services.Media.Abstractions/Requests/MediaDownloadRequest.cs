using System;
using System.IO;
using System.Threading;

namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Request data for <see cref="IMediaDownloader.DownloadAsync(MediaDownloadRequest, CancellationToken)"/>.
	/// </summary>
	public class MediaDownloadRequest
	{
		/// <summary>
		/// Uri to the media to download.
		/// </summary>
		public RemoteResourceDownloadInfo DownloadInfo { get; set; }

		/// <summary>
		/// Directory to which the media should be downloaded.
		/// </summary>
		public DirectoryInfo Directory { get; set; }

		/// <summary>
		/// Callback for when the media data is received from download request.
		/// </summary>
		public Action<MediaData> DataCallback { get; set; }

		/// <inheritdoc/>
		public override string ToString()
			=> $"{DownloadInfo} into {Directory}";
	}
}
