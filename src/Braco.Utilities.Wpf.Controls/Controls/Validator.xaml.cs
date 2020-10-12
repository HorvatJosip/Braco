using Braco.Utilities.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Interaction logic for Validator.xaml
    /// </summary>
    public partial class Validator : UserControl
    {
        /// <summary>
        /// Object to validate (Pass in property name). Can also be a binding to error message.
        /// </summary>
        public object Validate
        {
            get { return GetValue(ValidateProperty); }
            set { SetValue(ValidateProperty, value); }
		}

		/// <summary>
		/// Dependency property for <see cref="Validate"/>.
		/// </summary>

		public static readonly DependencyProperty ValidateProperty =
            DependencyProperty.Register(nameof(Validate), typeof(object), typeof(Validator), new PropertyMetadata(null, OnValidatePropertyChanged));

        /// <summary>
        /// Option for <see cref="StringToVisibilityConverter"/> parameter. Defaults to null which
        /// will result in default behavior (collapse when there is no error, show when there is).
        /// </summary>
        public string VisibilityOption
        {
            get { return (string)GetValue(VisibilityOptionProperty); }
            set { SetValue(VisibilityOptionProperty, value); }
        }

		/// <summary>
		/// Dependency property for <see cref="VisibilityOption"/>.
		/// </summary>
        public static readonly DependencyProperty VisibilityOptionProperty =
            DependencyProperty.Register(nameof(VisibilityOption), typeof(string), typeof(Validator), new PropertyMetadata(null));

		/// <summary>
		/// Creates an instance of the control.
		/// </summary>
		public Validator()
        {
            InitializeComponent();

			HorizontalAlignment = HorizontalAlignment.Center;
        }

		private static void OnValidatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Instance of the current class
            var instance = d as Validator;

            // Label that will show the error
            var textBlock = instance.lblError;

            // Updates label with the given content and sets its visibility based on it
            void UpdateLabel(object content)
            {
                textBlock.Visibility = (Visibility)StringToVisibilityConverter.Instance.Convert(content, null, instance.VisibilityOption, null);

				textBlock.Text = content?.ToString();
            }

            // Get the binding from the dependency property
            var binding = BindingOperations.GetBinding(d, e.Property);

            // If there is just a binding to the error message...
            if (binding != null)
                // Use the value that is stored in the bound property
                UpdateLabel(e.NewValue);

            // Otherwise...
            else
            {
                // Context that defines what is bound to the page or window
                var context = instance.DataContext;

                // Type of the item that is bound
                var type = context?.GetType();

                // Get name of the property that needs to be validated
                var toValidate = instance.Validate?.ToString();

                // Get the property that needs to be validated (in case a string is passed into Validate dependency property)
                var property = type.GetNestedPrield(context, toValidate);

                // Extract name from property or, if it's null, use the Validate property value
                var name = property?.Member.Name ?? toValidate;

				// Listen for validations to update the label
				type.GetEvent(nameof(Utilities.Validator.ValidationPerformed)).AddEventHandler(
					context,
					new EventHandler<ValidationPerformedEventArgs>((sender, args) =>
						UpdateLabel(args.ValidationErrors.Find(x => x.Member == name)?.ErrorString)
					)
				);
            }
        }
    }
}
