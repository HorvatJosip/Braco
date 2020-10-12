using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Base class for any multi value converters used in XAML.
    /// </summary>
    /// <typeparam name="Converter">Type of the converter that is inheriting this class.</typeparam>
    public abstract class BaseMultiValueConverter<Converter> : MarkupExtension, IMultiValueConverter
        where Converter : new()
    {
        /// <summary>
        /// Instance of the converter.
        /// </summary>
        public static Converter Instance { get; } = new Converter();

        /// <summary>
        /// Implementation of <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/>.
        /// </summary>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Implementation of <see cref="IMultiValueConverter.ConvertBack(object, Type[], object, CultureInfo)"/>.
        /// </summary>
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);

        /// <summary>
        /// Implementation of <see cref="MarkupExtension.ProvideValue(IServiceProvider)"/>.
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider) => Instance;
    }
}
