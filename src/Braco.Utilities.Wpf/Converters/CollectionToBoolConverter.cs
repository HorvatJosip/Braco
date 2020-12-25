using Braco.Utilities.Extensions;
using System;
using System.Collections;
using System.Globalization;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Converts state of the collection to <see cref="bool"/> based on
	/// it being empty or not.
	/// </summary>
	public class CollectionToBoolConverter : BaseConverter<CollectionToBoolConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is IEnumerable enumerable)
				return enumerable.IsNotNullOrEmpty();

			return false;
		}

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
