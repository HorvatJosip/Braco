using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Request data for <see cref="IMediaSplitter.SplitAsync(MediaSplitRequest, CancellationToken)"/>.
	/// </summary>
	public class MediaSplitRequest
	{
		/// <summary>
		/// Source file to split.
		/// </summary>
		public FileInfo SourceFile { get; set; }

		/// <summary>
		/// Destination (output) file to use once the media has been split.
		/// </summary>
		public FileInfo DestinationFile { get; set; }

		/// <summary>
		/// Starting time (if left null, start of the media will be used).
		/// </summary>
		public TimeSpan? Start { get; set; }

		/// <summary>
		/// Ending time (if left null, end of the media will be used).
		/// </summary>
		public TimeSpan? End { get; set; }

		/// <summary>
		/// Ranges to split in the middle of the media.
		/// </summary>
		public IEnumerable<TimeRange> SplitRanges { get; set; }
	}
}