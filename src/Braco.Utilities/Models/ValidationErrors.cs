using Braco.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace Braco.Utilities
{
    /// <summary>
    /// Validation errors for a member.
    /// </summary>
    public class ValidationErrors
    {
        /// <summary>
        /// Member on which the validation was performed.
        /// </summary>
        public string Member { get; }

        /// <summary>
        /// Errors that have been found on the member.
        /// </summary>
        public IList<string> Errors { get; } = new List<string>();

        /// <summary>
        /// Gets a string constructed from <see cref="Errors"/>.
        /// </summary>
        public string ErrorString => Errors.Join(Environment.NewLine);

		/// <summary>
		/// Creates validation errors for the given member.
		/// </summary>
		/// <param name="member"></param>
        public ValidationErrors(string member)
        {
            Member = member;
        }

		/// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is ValidationErrors results && Member == results.Member;

		/// <inheritdoc/>
        public override int GetHashCode()
            => new { Member }.GetHashCode();

		/// <inheritdoc/>
        public override string ToString() => $"{Member}: {ErrorString}";
    }
}