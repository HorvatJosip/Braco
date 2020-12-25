using System;
using System.Collections.Generic;
using System.Globalization;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for collecting values from a multi binding. If parameter exists, it will be added to resulting list.
	/// </summary>
	public class ValueCollectorMultiValueConverter : BaseMultiValueConverter<ValueCollectorMultiValueConverter>
	{
		/// <summary>
		/// Determines whether or not the values that are null should be removed from
		/// the final list that the converter returns.
		/// <para>The default value is true.</para>
		/// </summary>
		public bool RemoveNullValues { get; set; } = true;

		/// <inheritdoc/>
		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null) return null;

			var list = new List<object>(values);

			if (parameter != null)
			{
				list.Add(parameter);
			}

			if (RemoveNullValues)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i] == null)
					{
						list.RemoveAt(i);
					}
				}
			}

			return list;
		}

		/// <inheritdoc/>
		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
