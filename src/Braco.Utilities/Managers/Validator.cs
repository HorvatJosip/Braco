using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Braco.Utilities
{
    /// <summary>
    /// Collection of methods used for validating properties
    /// of an object.
    /// </summary>
    public class Validator
    {
        private readonly object _instance;
        private readonly ILocalizer _localizer;

        private bool _validationOnPropertyChangedSetup;

        /// <summary>
        /// Event that is fired after a validation has been performed.
        /// </summary>
        public event EventHandler<ValidationPerformedEventArgs> ValidationPerformed;

		/// <summary>
		/// Creates a new instance of the validator.
		/// </summary>
		/// <param name="instance">Instance which will be validated.</param>
		/// <param name="localizer">Localizer to use for the validator.</param>
        public Validator(object instance, ILocalizer localizer)
        {
            _instance = instance;
			_localizer = localizer;
        }

        /// <summary>
        /// Validates a property on the given object.
        /// </summary>
        /// <param name="target">Object to validate.</param>
        /// <param name="propertyName">Property to validate on the given object.</param>
        /// <returns>If the object is valid.</returns>
        public bool ValidateProperty(object target, string propertyName)
        {
            // If nothing was given...
            if (target == null || propertyName == null)
                // Signal invalid
                return false;

            // Get the type from the object
            var type = target.GetType();

            // Get the property by name
            var property = type.GetProperty(propertyName);

            // If it doesn't exist...
            if (property == null)
                // Signal invalid
                return false;

            // Get the validation attributes
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();

            // Get the value of the given property
            var value = property.GetValue(target);

            // Check if all of the values are valid according to the attributes
            return validationAttributes.All(attr => attr.IsValid(value));
        }

        /// <summary>
        /// Used to validate a property every time its value changes.
        /// </summary>
        /// <param name="propertyName">Property to track.</param>
        /// <param name="additionalChecks">Additional checks to perform on the property.</param>
        public void ValidateOnPropertyChanged(string propertyName, params MemberCheck[] additionalChecks)
        {
            // If it was already setup...
            if (_validationOnPropertyChangedSetup)
                // Bail
                return;

            // Find the property by name
            var prop = _instance.GetType().GetProperty(propertyName);

            // If no name is provided, use current instance, otherwise, get the value
            // of the property from the current instance
            var target = propertyName == null ? _instance : prop?.GetValue(_instance);

            // If it doesn't have value yet...
            if (target == null)
                // Subscribe to property changes
                ReflectionUtilities.ListenForPropertyChanges(_instance, propertyName, _ =>
                {
                    // Update the object value
                    target = prop.GetValue(_instance);

                    // If the desired property has some value...
                    if (target != null)
                        // Start validating every time it changes
                        ValidateOnChange();
                });

            // Otherwise...
            else
                // Just start validating every time it changes
                ValidateOnChange();

            void ValidateOnChange()
            {
                // Subscribe to property changes
                ReflectionUtilities.ListenForPropertyChanges(target, (_, propName) =>
                {
                    // Get the changed property by name
                    var property = target.GetType().GetProperty(propName);

                    // If it has value and some validation attributes...
                    if (property.GetValue(target) != null && property.GetCustomAttributes<ValidationAttribute>().Count() > 0)
                        // Validate it
                        Validate(target, additionalChecks);
                });
            }

            // Validation was setup, don't repeat this
            _validationOnPropertyChangedSetup = true;
        }

        /// <summary>
        /// Validates all of the properties on the view model.
        /// Returns true if the validation results in no errors.
        /// </summary>
        /// <param name="additionalChecks">Checks to perform aside from ones on properties.</param>
        /// <returns></returns>
        public bool Validate(params MemberCheck[] additionalChecks)
            => Validate(_instance, additionalChecks);

        /// <summary>
        /// Validates all of the properties on the given <paramref name="target"/>.
        /// Returns true if the validation results in no errors.
        /// </summary>
        /// <param name="target">Target object to validate.</param>
        /// <param name="additionalChecks">Checks to perform aside from ones on properties.</param>
        /// <returns></returns>
        public bool Validate(object target, params MemberCheck[] additionalChecks)
        {
            // Define the result as valid by default
            var valid = true;
            // Create a collection to pass to the event
            var validationErrors = new List<ValidationErrors>();
            // Get the target's type
            var type = target.GetType();

            void AddError(string member, string error)
            {
                // Find the errors for the given member
                var memberErrors = validationErrors.FirstOrDefault(x => x.Member == member);

                // If there aren't any yet...
                if (memberErrors == null)
                {
                    // Make a new instance for validation errors
                    memberErrors = new ValidationErrors(member);

                    // Add it to all validation errors
                    validationErrors.Add(memberErrors);
                }

                // Add the error related to the given member
                memberErrors.Errors.Add(error);
            }

            // Go through the properties of the target object
            foreach (var property in type.GetProperties())
            {
                // Skip property if it can be found in additional checks and we want to skip it
                if (additionalChecks.Any(check => check.Skip && check.Member == property.Name))
                    continue;

                // Get the validation attributes
                var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();

                // Value of the current property
                var value = property.GetValue(target);

                // Go through the attributes that need to be validated...
                validationAttributes.ForEach(attr =>
                {
                    // Construct the key for localizer
                    var localeKey = string.Join
                    (
                        "_",
                        type.Name,
                        property.Name,
                        attr.GetType().Name.Replace(nameof(Attribute), ""),
                        Message.Error
                    );

                    bool attrValid;

                    // If the attribute has to be given validation context...
                    if (attr.RequiresValidationContext)
                    {
                        // Create a list of validation results
                        var validationResults = new List<ValidationResult>();

                        // Validate it using the built in validator class 
                        // by giving the validation context
                        System.ComponentModel.DataAnnotations.Validator.TryValidateProperty
                        (
                            value: value,
                            validationContext: new ValidationContext(target) { MemberName = property.Name },
                            validationResults: validationResults
                        );

                        // Attribute is valid if there are no error messages
                        // Note: All() return true if the list is empty
                        attrValid = validationResults.All(result => result.ErrorMessage.IsNullOrWhiteSpace());
                    }

                    // Otherwise
                    else
                        // Just validate the attribute
                        attrValid = attr.IsValid(value);

                    // If property value isn't valid according to the attribute...
                    if (attrValid == false)
                    {
                        // Add error to the list
                        AddError(property.Name, _localizer[localeKey]);

                        // If validity is currently true...
                        if (valid)
                            // Change it to false
                            valid = false;
                    }
                });
            }

            // Go through additional checks
            foreach (var (check, errorMessage, member, skip) in additionalChecks)
                // If the check failed...
                if (check?.Compile().Invoke() != true)
                {
                    // Add error to the list
                    AddError(member, errorMessage);

                    // If validity is currently true...
                    if (valid)
                        // Change it to false
                        valid = false;
                }

            // Signal that the validation has been performed
            ValidationPerformed?.Invoke(_instance, new ValidationPerformedEventArgs
            {
                // Pass in the errors from validations that have been performed
                ValidationErrors = validationErrors
            });

            // Return the result of validations
            return valid;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Validator for {_instance}";
	}
}
