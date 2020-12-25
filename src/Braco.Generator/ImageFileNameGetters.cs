using Braco.Utilities;
using Braco.Utilities.Extensions;
using System.Collections.Generic;

namespace Braco.Generator
{
	public static class ImageFileNameGetters
	{
		public static string SortIcon(object[] parameters)
		{
			string icon = null;

			if 
			(
				parameters.IsNotNullOrEmpty() &&
				parameters[2] is List<object> values &&
				values.Count == 2 &&
				values[0] is LocalizedTableViewModel table &&
				values[1] is string columnName
			)
			{
				var column = table.DataManager.GetDisplayColumn(columnName);

				icon = column.SortDirection switch
				{
					SortDirection.Ascending => ResourceKeys.SortDescendingIcon,
					SortDirection.Descending => ResourceKeys.SortAscendingIcon,
					_ => ResourceKeys.SortDefaultIcon
				};
			}

			return icon;
		}
	}
}
