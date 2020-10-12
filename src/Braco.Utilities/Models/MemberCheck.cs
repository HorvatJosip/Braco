using System;
using System.Linq.Expressions;

namespace Braco.Utilities
{
    /// <summary>
    /// Used for performing a check on the specified member.
    /// </summary>
    public class MemberCheck
    {
		/// <summary>
		/// Check to perform.
		/// </summary>
        public Expression<Func<bool>> Check { get; set; }

		/// <summary>
		/// Error message in case check fails.
		/// </summary>
        public string ErrorMessage { get; set; }

		/// <summary>
		/// Member to check.
		/// </summary>
        public string Member { get; set; }

		/// <summary>
		/// Should the check be skipped on this member?
		/// </summary>
        public bool Skip { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
        public MemberCheck() { }

		/// <summary>
		/// Defines the member check using the given values.
		/// </summary>
		/// <param name="check">check to perform.</param>
		/// <param name="errorMessage">error message in case check fails.</param>
		/// <param name="member">member to check.</param>
		/// <param name="skip">should the check be skipped on this member?</param>
		public MemberCheck(Expression<Func<bool>> check, string errorMessage, string member, bool skip = false)
        {
            Check = check;
            ErrorMessage = errorMessage;
            Member = member;
            Skip = skip;
        }

		/// <summary>
		/// Used for deconstructing the model.
		/// </summary>
		/// <param name="check"></param>
		/// <param name="errorMessage"></param>
		/// <param name="member"></param>
		/// <param name="skip"></param>
        public void Deconstruct(out Expression<Func<bool>> check, out string errorMessage, out string member, out bool skip)
        {
            check = Check;
            errorMessage = ErrorMessage;
            member = Member;
            skip = Skip;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Check for {Member} (Skip: {Skip})";
	}
}