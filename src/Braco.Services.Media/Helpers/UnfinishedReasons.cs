namespace Braco.Services.Media
{
	/// <summary>
	/// List of reasons a request didn't finish.
	/// </summary>
	public class UnfinishedReasons
	{
		/// <summary>
		/// The provided YouTube uri is invalid.
		/// </summary>
		public const string InvalidYouTubeUri = nameof(InvalidYouTubeUri);

		/// <summary>
		/// Requested video wasn't found.
		/// </summary>
		public const string VideoNotFound = nameof(VideoNotFound);

		/// <summary>
		/// There was an error writing video's bytes to the file.
		/// </summary>
		public const string VideoFileWritingError = nameof(VideoFileWritingError);

		/// <summary>
		/// An error occurred while downloading the video.
		/// </summary>
		public const string VideoDownloadError = nameof(VideoDownloadError);

		/// <summary>
		/// An error occurred while downloading the audio.
		/// </summary>
		public const string AudioDownloadError = nameof(AudioDownloadError);

		/// <summary>
		/// An error occurred while converting video to audio.
		/// </summary>
		public const string VideoToAudioConversionError = nameof(VideoToAudioConversionError);

		/// <summary>
		/// An error occurred while splitting the audio.
		/// </summary>
		public const string AudioSplitError = nameof(AudioSplitError);
	}
}
