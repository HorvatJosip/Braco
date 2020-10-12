using System;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Handles auth logic and stores the currently logged in user.
	/// </summary>
	public interface IAuthService
    {
        /// <summary>
        /// String representation of the currently logged in user.
        /// </summary>
        string CurrentUser { get; }

        /// <summary>
        /// Defines if the user is currently authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Location where the user should be redirected in case
        /// they aren't authenticated.
        /// </summary>
        string UnauthenticatedLocation { get; }

        /// <summary>
        /// Raised whenever an auth action occurrs.
        /// </summary>
        event EventHandler<AuthEventArgs> AuthActionOccurred;

        /// <summary>
        /// Tries to register a new user.
        /// </summary>
        /// <param name="identifier">Identifier (username or email) to use for the user.</param>
        /// <param name="password">Password to register for the user.</param>
        /// <returns>If the user was registered or not.</returns>
        bool Register(string identifier, string password);

        /// <summary>
        /// Tries to log the user in with the given credentials.
        /// </summary>
        /// <param name="identifier">Identifier (username or email) of the user to check.</param>
        /// <param name="password">Password of the user to check.</param>
        /// <returns>If the user was authenticated or not.</returns>
        bool LogIn(string identifier, string password);

        /// <summary>
        /// Tries to log the user out.
        /// </summary>
        /// <returns>If the user was logged out or not.</returns>
        bool LogOut();
    }
}
