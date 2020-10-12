using Braco.Services;
using System;
using System.Globalization;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Used for converting enums to localized strings and vice versa.
	/// </summary>
	public class EnumToStringConverter : BaseConverter<EnumToStringConverter>
	{
		/// <summary>
		/// Suffix for "all" option of enums.
		/// </summary>
		public const string EnumAllExtension = "_All";
		/// <summary>
		/// Fallback localization key for "all" option of enums.
		/// </summary>
		public const string EnumAllFallback = "Enum" + EnumAllExtension;

		/// <inheritdoc/>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null
				? LocalizedValueForAll(parameter as Type)
				: DI.Localizer[$"{value.GetType().Name}_{value}"];
		}

		/// <inheritdoc/>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var enumType = targetType.IsEnum
				? targetType
				: targetType.GetGenericArguments()[0];

			if(value is string @enum)
				foreach (var enumValue in Enum.GetValues(enumType))
					if(@enum == DI.Localizer[$"{enumType.Name}_{enumValue}"])
						return enumValue;
			
			return null;
		}

		/// <summary>
		/// Gets a localized value for the 'all option' for
		/// the enum of given type (If the type is null,
		/// a fallback value is given).
		/// </summary>
		/// <param name="type">Type of enum.</param>
		public static string LocalizedValueForAll(Type type)
		{
			string result = null;

			if(type != null)
			{
				result = DI.Localizer[$"{type.Name}{EnumAllExtension}"];
			}

			return result ?? DI.Localizer[EnumAllFallback];
		}
	}
}
