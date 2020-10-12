namespace Braco.Services.Media.Abstractions
{
	/// <summary>
	/// Contains information for downloading a remote resource.
	/// </summary>
	public class RemoteResourceDownloadInfo
	{
		/// <summary>
		/// Remote location of the resource.
		/// </summary>
		public string Uri { get; set; }

		/// <summary>
		/// How many chunks to download at a time.
		/// </summary>
		public int ChunkSize { get; set; }
	}
}
