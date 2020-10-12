using Braco.Services.Abstractions;
using System.IO;
using System.Threading;

namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Response data for <see cref="IMediaSplitter.SplitAsync(MediaSplitRequest, CancellationToken)"/>.
	/// </summary>
	public class MediaSplitResponse : Response<MediaSplitResponse>
	{
		/// <summary>
		/// Split media file.
		/// </summary>
		public FileInfo SplitFile { get; set; }

		/// <summary>
		/// Source file that was used for splitting.
		/// </summary>
		public FileInfo SourceFile { get; set; }
	}
}