using Braco.Services.Abstractions;
using System;

namespace Braco.Utilities.Wpf
{
    /// <summary>
    /// Using this on a <see cref="PageViewModel"/> will make sure
    /// that the user is authenticated (<see cref="IAuthService.IsAuthenticated"/>).
    /// If the user isn't authenticated, they will be redirected to
	/// <see cref="IAuthService.UnauthenticatedLocation"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizeAttribute : Attribute
	{
		/// <inheritdoc/>
		public override string ToString()
			=> $"Authorize";
	}
}
