using System;
using System.Windows;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Wrapper around vanilla attached property to make
    /// the usage simpler and more readable.
    /// </summary>
    /// <typeparam name="TOwner">Owner class that this property will attach to.</typeparam>
    /// <typeparam name="TPropertyType">Type of the property that will be attached to the parent.</typeparam>
    public abstract class BaseAttachedProperty<TOwner, TPropertyType>
        where TOwner : BaseAttachedProperty<TOwner, TPropertyType>, new()
    {
        /// <summary>
        /// Name of the attached property.
        /// </summary>
        public const string PropertyName = "Value";

        /// <summary>
        /// Fired when the value of the attached property changes.
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged;

        /// <summary>
        /// Singleton of the owner of the property.
        /// </summary>
        public static TOwner Instance { get; } = new TOwner();

        #region Attached Property Definition

        /// <summary>
        /// The property that will be attached to the owner.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            name: PropertyName,
            propertyType: typeof(TPropertyType),
            // Note: TOwner can't be set as the owner because owner is a class that has
            // defined the property, which is in this case the current class.
            // It is only tied to this class (static keyword), owner classes will
            // just implement the logic to react to property value changes.
            ownerType: typeof(BaseAttachedProperty<TOwner, TPropertyType>),
            defaultMetadata: new PropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged))
        );

        /// <summary>
        /// Callback for when the value of the property changes.
        /// </summary>
        /// <param name="d">UI element that had its property changed.</param>
        /// <param name="e">Arguments for the event.</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Call event listeners
            Instance.ValueChanged?.Invoke(d, e);
            Instance.OnValueChanged(d, e);
        }

        /// <summary>
        /// Gets the value stored in the attached property of the given element.
        /// </summary>
        /// <param name="d">The element for which to get the value of the attached property.</param>
        /// <returns></returns>
        public static TPropertyType GetValue(DependencyObject d) => (TPropertyType)d.GetValue(ValueProperty);

        /// <summary>
        /// Sets the value of the attached property of the given element.
        /// </summary>
        /// <param name="d">Element for which to set the attached property.</param>
        /// <param name="value">Value used for setting the attached property.</param>
        public static void SetValue(DependencyObject d, TPropertyType value) => d.SetValue(ValueProperty, value);

        #endregion

        /// <summary>
        /// Method that is called when the attached property changes.
        /// </summary>
        /// <param name="sender">UI element that had its property changed.</param>
        /// <param name="e">Arguments for the event.</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }
    }
}