using System;

namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Data received upon sending request to download media.
	/// </summary>
	public class MediaData
	{
		/// <summary>
		/// Media title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Duration of the media.
		/// </summary>
		public TimeSpan? Duration { get; set; }
	}
}