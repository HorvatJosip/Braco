namespace Braco.Services.Abstractions
{
    /// <summary>
    /// Arguments for the <see cref="IAuthService"/> actions.
    /// </summary>
    public class AuthEventArgs : System.EventArgs
    {
        /// <summary>
        /// Action that occurred.
        /// </summary>
        public AuthAction Action { get; }

		/// <summary>
		/// Creates an instance of the arguments.
		/// </summary>
		/// <param name="action">Action that was performed.</param>
        public AuthEventArgs(AuthAction action)
        {
            Action = action;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"Auth action: {Action}";
	}
}
