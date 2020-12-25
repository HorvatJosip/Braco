using Braco.Utilities.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Braco.Generator
{
	[TypeConverter(typeof(ObservableCollection<FileViewModel>))]
	public class ExistingProjectsConverter : TypeConverter
	{
		public const string Separator = "|";

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is not string separatedValues) throw new NotSupportedException("Only strings are supported.");

			return new ObservableCollection<FileViewModel>
			(
				collection: separatedValues
					.Split(Separator)
					.Where(value => value.IsNotNullOrEmpty())
					.Select(FileViewModel.FromPath)
			);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is not ObservableCollection<FileViewModel> collection || destinationType != typeof(string)) throw new NotSupportedException("Only converting to strings is supported.");

			return collection.Select(item => item.Path).Join(Separator);
		}
	}
}
