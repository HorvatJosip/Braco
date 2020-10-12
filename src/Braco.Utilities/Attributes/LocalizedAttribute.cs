using System;

namespace Braco.Utilities
{
	/// <summary>
	/// Attribute used for specifying that a field or a property is localized.
	/// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LocalizedAttribute : Attribute
    {
		/// <summary>
		/// Key for localization.
		/// </summary>
        public string Key { get; set; }

		/// <inheritdoc/>
		public override string ToString()
			=> $"Localized [{Key}]";
	}
}
