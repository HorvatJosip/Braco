using Braco.Services.Abstractions;
using System.IO;
using System.Threading;

namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Response data for <see cref="IMediaDownloader.DownloadAsync(MediaDownloadRequest, CancellationToken)"/>.
	/// </summary>
	public class MediaDownloadResponse : Response<MediaDownloadResponse>
	{
		/// <summary>
		/// Media file.
		/// </summary>
		public FileInfo File { get; set; }

		/// <summary>
		/// Data about the media.
		/// </summary>
		public MediaData Data { get; set; }
	}
}
