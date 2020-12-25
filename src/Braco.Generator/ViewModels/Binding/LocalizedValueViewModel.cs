using Braco.Utilities;

namespace Braco.Generator
{
	public class LocalizedValueViewModel
	{
		[TableColumn(LocalizationKeys.LocalizationTable_KeyColumn, 0)]
		public string Key { get; set; }

		[TableColumn(LocalizationKeys.LocalizationTable_ValueColumn, 1)]
		public string Value { get; set; }

		public LocalizedValueViewModel() { }

		public LocalizedValueViewModel(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}
}
