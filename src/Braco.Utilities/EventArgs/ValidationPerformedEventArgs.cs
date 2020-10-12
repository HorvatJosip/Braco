using System.Collections.Generic;

namespace Braco.Utilities
{
    /// <summary>
    /// Event arguments for the event that will be raised
    /// after a validation has been performed.
    /// </summary>
    public class ValidationPerformedEventArgs : System.EventArgs
    {
        /// <summary>
        /// List of validations for every property.
        /// </summary>
        public List<ValidationErrors> ValidationErrors { get; set; }
    }
}
