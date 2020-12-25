namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Request that contains data for drop event that listens for file drops.
	/// </summary>
	/// <param name="DataSource">Data source of the object whose drop event fired.</param>
	/// <param name="FilePaths">Paths to files that were dropped.</param>
	public record FileDropCommandRequest(object DataSource, string[] FilePaths);
}
