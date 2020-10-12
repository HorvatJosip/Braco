using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// <see cref="ResourceGetter"/> for tooltips by image names.
	/// </summary>
	public class ToolTipGetter : ResourceGetter
	{
		/// <summary>
		/// Map of keys for tooltips based on image file names.
		/// </summary>
		protected readonly IDictionary<string, string> _toolTipKeyMap;

		/// <summary>
		/// Creates the getter using the initial dictionary of values.
		/// </summary>
		/// <param name="toolTipKeyMap">Map of keys for tooltips based on image file names.</param>
		public ToolTipGetter(IDictionary<string, string> toolTipKeyMap)
			=> _toolTipKeyMap = toolTipKeyMap ?? new Dictionary<string, string>();

		/// <summary>
		/// Adds a tooltip key based on image file name.
		/// <para>Note: null or empty strings not allowed.</para>
		/// </summary>
		/// <param name="imageFileName">File name of the image.</param>
		/// <param name="toolTipKey">Key for the tooltip.</param>
		public void AddKeyGetter(string imageFileName, string toolTipKey)
		{
			if (imageFileName.IsNullOrEmpty() || toolTipKey.IsNullOrEmpty()) return;

			_toolTipKeyMap[imageFileName] = toolTipKey;
		}

		/// <summary>
		/// Adds a range of tooltip keys based on image file names.
		/// <para>Note: null or empty strings not allowed.</para>
		/// </summary>
		/// <param name="toolTipKeyMap">Map of keys for tooltips based on image file names.</param>
		public void AddKeyGetterRange(params (string imageFileName, string toolTipKey)[] toolTipKeyMap)
			=> AddKeyGetterRange
			(
				toolTipKeyMap.Select
				(
					tuple => new KeyValuePair<string, string>(tuple.imageFileName, tuple.toolTipKey)
				)
			);

		/// <summary>
		/// Adds a range of tooltip keys based on image file names.
		/// <para>Note: null or empty strings not allowed.</para>
		/// </summary>
		/// <param name="toolTipKeyMap">Map of keys for tooltips based on image file names.</param>
		public void AddKeyGetterRange(IEnumerable<KeyValuePair<string, string>> toolTipKeyMap)
			=> toolTipKeyMap.ForEach(kvp => AddKeyGetter(kvp.Key, kvp.Value));

		/// <summary>
		/// Gets tooltip key based on the image file name.
		/// </summary>
		/// <param name="imageFileName">File name of the image whose tooltip key
		/// needs to be found.</param>
		/// <returns>Localization key for the tooltip based on image file name.</returns>
		public virtual string GetToolTipKey(string imageFileName)
			=> _toolTipKeyMap.TryGetValue(imageFileName, out var toolTipKey) ? toolTipKey : null;
	}
}