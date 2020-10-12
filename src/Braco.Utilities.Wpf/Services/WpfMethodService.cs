using Braco.Services.Abstractions;
using System;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Implementation of <see cref="IMethodService"/> using
	/// WPF's dispatchers.
	/// </summary>
    public class WpfMethodService : IMethodService
    {
		/// <inheritdoc/>
        public T InvokeOnUIThread<T>(Func<T> method)
            => Application.Current.Dispatcher.Invoke(method);

		/// <inheritdoc/>
        public void InvokeOnUIThread(Action method)
            => Application.Current.Dispatcher.Invoke(method);
    }
}
