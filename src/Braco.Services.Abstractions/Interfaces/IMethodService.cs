using System;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Defines specific method invokation logic.
	/// </summary>
    public interface IMethodService
    {
        /// <summary>
        /// Invokes the given method on UI thread.
        /// </summary>
        /// <typeparam name="T">Return type for the method.</typeparam>
        /// <param name="method">Method to invoke on UI thread.</param>
        /// <returns>Method result.</returns>
        T InvokeOnUIThread<T>(Func<T> method);

        /// <summary>
        /// Invokes the given method on UI thread.
        /// </summary>
        /// <param name="method">Method to invoke on UI thread.</param>
        void InvokeOnUIThread(Action method);
    }
}
