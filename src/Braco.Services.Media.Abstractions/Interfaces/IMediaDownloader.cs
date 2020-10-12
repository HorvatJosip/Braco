using System.Threading;
using System.Threading.Tasks;

namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Defines a service that downloads media from a source.
	/// </summary>
	public interface IMediaDownloader
	{
		/// <summary>
		/// Downloads media asynchronously.
		/// </summary>
		/// <param name="request">Data required for downloading the media.</param>
		/// <param name="cancellationToken">Token used for signalling cancellation.</param>
		/// <returns>Information about downloaded media or null if it failed.</returns>
		Task<MediaDownloadResponse> DownloadAsync(MediaDownloadRequest request, CancellationToken cancellationToken);

		/// <summary>
		/// Determines if the uri is in valid format.
		/// <para>If there is a minor tweak required, true will be returned and
		/// the uri given out as <paramref name="formatted"/> parameter.</para>
		/// </summary>
		/// <param name="uri">Uri to check if it is valid.</param>
		/// <param name="formatted">Tweaked uri to be correct.</param>
		/// <returns>If the given uri is in valid format or not.</returns>
		bool IsUriInValidFormat(string uri, out string formatted);
	}
}
