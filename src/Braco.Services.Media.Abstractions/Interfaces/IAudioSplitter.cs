using System.Threading;
using System.Threading.Tasks;

namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Defines a service that splits media.
	/// </summary>
	public interface IMediaSplitter
	{
		/// <summary>
		/// Splits media asynchronously.
		/// </summary>
		/// <param name="request">Splitting specification.</param>
		/// <param name="cancellationToken">Token used for signalling cancellation.</param>
		/// <returns>Information about split media or null if it failed.</returns>
		Task<MediaSplitResponse> SplitAsync(MediaSplitRequest request, CancellationToken cancellationToken);
	}
}
