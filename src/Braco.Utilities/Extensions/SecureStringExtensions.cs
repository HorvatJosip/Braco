using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Braco.Utilities.Extensions
{
	/// <summary>
	/// Extensions for <see cref="SecureString"/>.
	/// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Takes in a <see cref="SecureString"/> and returns its data
        /// converted to <see cref="string"/>.
        /// </summary>
        /// <param name="value"><see cref="SecureString"/> to unsecure.</param>
        /// <returns>Unsecured <see cref="SecureString"/>.</returns>
        public static string Unsecure(this SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;

            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
