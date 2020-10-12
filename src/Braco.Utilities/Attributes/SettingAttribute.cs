using System;

namespace Braco.Utilities
{
	/// <summary>
	/// Indicates that the decorated property is a setting
	/// that can be retrieved on load and change the settings
	/// when its value changes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class SettingAttribute : Attribute
	{
		/// <summary>
		/// Specifies that the property should be set from the settings
		/// on load. Defaults to true.
		/// </summary>
		public bool Load { get; set; } = true;

		/// <summary>
		/// Specifies that whenever the property's value changes, the
		/// setting should be changed as well. Defaults to true.
		/// </summary>
		public bool UpdateOnValueChanged { get; set; } = true;

		/// <summary>
		/// Key used for identifying the setting. If not specified,
		/// property name will be used.
		/// </summary>
		public string Key { get; set; }
	}
}
