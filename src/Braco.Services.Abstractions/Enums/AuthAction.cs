namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Auth actions that might be executed.
	/// </summary>
    public enum AuthAction
    {
		/// <summary>
		/// Logging in.
		/// </summary>
        LogIn,

		/// <summary>
		/// Logging out.
		/// </summary>
		LogOut,

		/// <summary>
		/// Registering.
		/// </summary>
		Register,
    }
}
