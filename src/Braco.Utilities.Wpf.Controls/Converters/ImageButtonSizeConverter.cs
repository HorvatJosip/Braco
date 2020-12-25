using Braco.Utilities.Extensions;
using System;
using System.Globalization;
using System.Linq;

namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Used for converting image button size string into an actual size.
	/// The parameter defines if we are working with width (<see cref="bool.TrueString"/>) or
	/// height (<see cref="bool.FalseString"/>).
	/// </summary>
	public class ImageButtonSizeConverter : BaseConverter<ImageButtonSizeConverter>
	{
		/// <summary>
		/// Used for separating values when specifying size
		/// </summary>
		public const string SizeSeparator = "x";

		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is string size && bool.TryParse(parameter as string, out var isWidth))
			{
				var parts = size
					.ToLower()
					.WithoutWhiteSpace()
					.Split(SizeSeparator)
					.Select(part => double.TryParse(part, out var size) ? size : (double?)null)
					.Where(part => part.HasValue && part.Value >= 0)
					.Select(part => part.Value)
					.ToList();

				if (parts.Count == 1) parts.Add(parts[0]);

				if (parts.Count != 2)
				{
					throw new Exception($"You must specify two numbers separated by {SizeSeparator}. Also, they have to be greater than or equal to 0.");
				}

				return parts[isWidth ? 0 : 1];
			}

			return double.NaN;
		}

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
