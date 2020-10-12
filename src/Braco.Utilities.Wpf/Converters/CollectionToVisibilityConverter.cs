using Braco.Utilities.Extensions;
using System;
using System.Collections;
using System.Globalization;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Converts state of the collection to <see cref="Visibility"/> based on
	/// it being empty or not.
	/// </summary>
	public class CollectionToVisibilityConverter : BaseConverter<CollectionToVisibilityConverter>
	{
		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is IEnumerable enumerable)
				return VisibilityHelpers.Convert(enumerable.IsNotNullOrEmpty(), parameter);

			return null;
		}

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
