using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Braco.Utilities.Extensions
{
	/// <summary>
	/// Extensions for <see cref="Stream"/>.
	/// </summary>
	public static class StreamExtensions
	{
		/// <summary>
		/// Writes a chunk of bytes from start to finish into the stream.
		/// </summary>
		/// <param name="stream">Stream into which to write the bytes.</param>
		/// <param name="chunk">Chunk of bytes to write to the stream.</param>
		public static void WriteChunk(this Stream stream, byte[] chunk)
			=> stream.Write(chunk, 0, chunk.Length);

		/// <summary>
		/// Writes a chunk of bytes from start to finish into the stream asynchronously.
		/// </summary>
		/// <param name="stream">Stream into which to write the bytes.</param>
		/// <param name="chunk">Chunk of bytes to write to the stream.</param>
		/// <returns></returns>
		public static async Task WriteChunkAsync(this Stream stream, byte[] chunk)
			=> await stream.WriteAsync(chunk, 0, chunk.Length);

		/// <summary>
		/// Writes given <paramref name="bytes"/> to the <paramref name="stream"/>
		/// in chunks of <paramref name="chunkSize"/>.
		/// </summary>
		/// <param name="stream">Stream to write to in chunks.</param>
		/// <param name="bytes">Collection of bytes to write in chunks into the stream.</param>
		/// <param name="chunkSize">Number of bytes that will be written to the stream at once.</param>
		/// <param name="cancellationToken">Token that can signal cancellation of writing operation.</param>
		/// <param name="progress">Progress reporting instance.</param>
		/// <returns>True if all of the bytes have been written to the stream. False if the cancellation was requested by the token.</returns>
		public static async Task<bool> WriteInChunksAsync(this Stream stream, byte[] bytes, int chunkSize, CancellationToken cancellationToken = default, PercentageProgress progress = default)
		{
			if (stream == null) throw new NullReferenceException();
			if (bytes == null) throw new ArgumentNullException(nameof(bytes));
			if (chunkSize < 1) throw new ArgumentException("Chunk size must be at least 1.", nameof(chunkSize));

			// Start from first byte
			var position = 0;

			// Setup the progress maximum
			progress?.Initialize(bytes.Length);

			// Go until the end
			while (position < bytes.Length)
			{
				// If the task was cancelled...
				if (cancellationToken.IsCancellationRequested)
				{
					// Close the stream
					stream.Close();

					// Signal that the writing was interrupted
					return false;
				}

				// Write current chunk
				await stream.WriteAsync(bytes, position, Math.Min(chunkSize, bytes.Length - position));

				// Move to the next chunk
				position += chunkSize;

				// Report the progress we've done this chunk
				progress?.Report(position);
			}

			// Signal that the writing finished
			return true;
		}
	}
}
