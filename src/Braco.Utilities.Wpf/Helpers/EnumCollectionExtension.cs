using Braco.Services;
using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Markup extensions for getting a collection of values for
    /// the specified enum type.
    /// </summary>
    public class EnumCollectionExtension : MarkupExtension
	{
		/// <summary>
		/// Type of enum.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// Indicates if the first option be "all".
		/// </summary>
		public bool PrependTheAllOption { get; set; } = true;

		/// <summary>
		/// Creates an instance of the extension.
		/// </summary>
		/// <param name="type">Type of enum to use</param>
		public EnumCollectionExtension(Type type)
		{
			Type = type;
		}

		/// <inheritdoc/>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var values = new List<string>();

			if (PrependTheAllOption)
				values.Add(EnumToStringConverter.LocalizedValueForAll(Type));

			foreach (var name in Enum.GetNames(Type))
				values.Add(DI.Localizer[$"{Type.Name}_{name}"]);

			return values;
		}
	}
}
