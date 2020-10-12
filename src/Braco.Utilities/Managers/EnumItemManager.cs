using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Braco.Utilities
{
	/// <summary>
	/// Used for localization of enums.
	/// </summary>
    public class EnumItemManager
    {
        private readonly Dictionary<Type, List<EnumItem>> _values = new Dictionary<Type, List<EnumItem>>();
        private readonly ILocalizer _localizer;

		/// <summary>
		/// Creates a new instance the manager.
		/// </summary>
		/// <param name="localizer">Localizer to use.</param>
        public EnumItemManager(ILocalizer localizer)
        {
            _localizer = localizer;
        }

		/// <summary>
		/// Used for extracting localized values from an enum.
		/// </summary>
		/// <typeparam name="TEnum">Enum type.</typeparam>
		/// <returns>Collection of localized strings from the given enum.</returns>
        public IEnumerable<string> ExtractLocalizedValues<TEnum>() where TEnum : Enum
        {
            var values = new List<string>();

            var type = typeof(TEnum);

            _values[type] = new List<EnumItem>();

            var localizedPrields = type.GetPrields(false).Where(x => Attribute.IsDefined(x.Member, typeof(LocalizedAttribute)));

            foreach (var prield in localizedPrields)
            {
                var localizedAttribute = prield.GetAttribute<LocalizedAttribute>();

                var item = new EnumItem
                {
                    LocalizedString = _localizer[localizedAttribute.Key ?? prield.Member.Name],
                    Value = (int)prield.GetValue(null)
                };

                _values[type].Add(item);
                values.Add(item.LocalizedString);
            }

            return values;
        }

		/// <summary>
		/// Gets information about the enum based on its localized variant.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum.</typeparam>
		/// <param name="value">Localized value of the enum.</param>
		/// <returns>Information about the enum based on its localized variant.</returns>
		public EnumItem GetEnumItemForString<TEnum>(string value) where TEnum : Enum
        {
			if (!_values.ContainsKey(typeof(TEnum)))
			{
				ExtractLocalizedValues<TEnum>();
			}

            if (!_values.TryGetValue(typeof(TEnum), out List<EnumItem> items))
            {
                return default;
            }

            var result = items.Find(x => x.LocalizedString == value);

            return result;
        }

		/// <summary>
		/// Gets the enum value based on its localized variant.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum.</typeparam>
		/// <param name="value">Localized value of the enum.</param>
		/// <returns>Enum value based on its localized variant.</returns>
		public TEnum GetEnumForString<TEnum>(string value) where TEnum : Enum
        {
            var enumItem = GetEnumItemForString<TEnum>(value);

            if (enumItem == null) return default;

            var result = (TEnum)Enum.ToObject(typeof(TEnum), enumItem.Value);

            return result;
        }

		/// <summary>
		/// Gets a localized version for the enum value.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum.</typeparam>
		/// <param name="value">Value for which to fetch the localized version.</param>
		/// <returns>Localized version for the given enum value.</returns>
		public string GetStringForEnum<TEnum>(TEnum value) where TEnum : Enum
		{
			if (!_values.ContainsKey(typeof(TEnum)))
			{
				ExtractLocalizedValues<TEnum>();
			}

			object enumValue = value;

            var intValue = (int)enumValue;

            if (_values.TryGetValue(typeof(TEnum), out List<EnumItem> items))
            {
                var result = items.Find(x => x.Value == intValue).LocalizedString;

                return result;
            }

            return null;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> _values.Join(" | ");
	}
}
