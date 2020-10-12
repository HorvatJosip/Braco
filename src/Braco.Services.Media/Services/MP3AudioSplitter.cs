using Braco.Services.Abstractions;
using Braco.Services.Media.Abstractions;
using Braco.Utilities.Extensions;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Braco.Services.Media
{
	/// <summary>
	/// Implementation of <see cref="IMediaSplitter"/> that works with MP3 files.
	/// </summary>
	public class MP3AudioSplitter : IMediaSplitter
	{
		/// <summary>
		/// Extension for MP3 files.
		/// </summary>
		public const string Mp3Extension = ".mp3";

		private const string tempFileNameSuffix = "_OLD_";

		/// <inheritdoc/>
		public async Task<MediaSplitResponse> SplitAsync(MediaSplitRequest request, CancellationToken cancellationToken)
		{
			try
			{
				// Check if name changed
				var sameFileName = request.SourceFile.FullName == request.DestinationFile.FullName;

				string sourceFilePath;

				// If the name didn't change...
				if (sameFileName)
				{
					// Rename the file to something unique, but identifiable
					sourceFilePath = request.SourceFile.FullName.Replace
					(
						oldValue: Mp3Extension,
						newValue: $"{tempFileNameSuffix}{Guid.NewGuid()}{Mp3Extension}"
					);

					File.Move(request.SourceFile.FullName, sourceFilePath);
				}

				// Otherwise...
				else
				{
					// Just keep the original name
					sourceFilePath = request.SourceFile.FullName;
				}

				// Create the response
				var response = new MediaSplitResponse
				{
					SourceFile = new FileInfo(sourceFilePath),
					SplitFile = request.DestinationFile,
					Messages = new List<Message>()
				};

				// Create an MP3 reader
				using var mp3Reader = new Mp3FileReader(sourceFilePath);

				// If there is already a file with same path...
				if (request.DestinationFile.Exists)
				{
					// Warn about overwriting it
					response.Messages.Add(Message.FromWarning($"File already exists ({request.DestinationFile.FullName}). Overwriting it."));

					// Delete it
					File.Delete(request.DestinationFile.FullName);
				}

				// Open destination file for writing
				using var writer = File.OpenWrite(request.DestinationFile.FullName);

				// Flag that indicates if we are on the first frame
				var first = true;
				while (true)
				{
					// Read the next frame
					var frame = mp3Reader.ReadNextFrame();

					// If there are no more frames, quit
					if (frame == null) break;

					// If this is the first chunk...
					if (first)
					{
						// Write it so that the file can be correct
						await writer.WriteChunkAsync(frame.RawData);

						first = false;
						continue;
					}

					// Get the current time
					var currentTime = mp3Reader.CurrentTime;

					// If we reached the end, stop writing to file
					if (request.End.HasValue && currentTime >= request.End) break;

					// If we aren't at start yet, skip
					if (request.Start.HasValue && currentTime < request.Start) continue;

					// If we are at one of the split ranges, skip
					if (request.SplitRanges?.Any(range => range.Contains(currentTime)) == true) continue;

					// Write the current frame to the file
					await writer.WriteChunkAsync(frame.RawData);
				}

				// Return the response
				return response;
			}
			catch (Exception ex)
			{
				return MediaSplitResponse.FromErrors(UnfinishedReasons.AudioSplitError, $"{ex}");
			}
		}
	}
}
