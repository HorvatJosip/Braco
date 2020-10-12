using Braco.Utilities.Extensions;
using System;
using System.Windows.Input;

namespace Braco.Utilities
{
	/// <summary>
	/// Command used for executing an action.
	/// </summary>
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Func<object, bool> _canExecute;
		private bool _canExecuteValue;

		#region Constructors

		/// <summary>
		/// Creates a relay command with specific action.
		/// </summary>
		/// <param name="execute">Action to execute.</param>
		public RelayCommand(Action execute) => _execute = obj => execute?.Invoke();

		/// <summary>
		/// Creates a relay command with specific action.
		/// </summary>
		/// <param name="execute">Action to execute.</param>
		public RelayCommand(Action<object> execute) => _execute = execute;

		/// <summary>
		/// Creates a relay command with specific action.
		/// </summary>
		/// <param name="execute">Action to execute.</param>
		/// <param name="canExecute">Method that determines if the command can execute.</param>
		public RelayCommand(Action execute, Func<bool> canExecute) : this(execute)
			=> _canExecute = canExecute == null ? null : new Func<object, bool>(obj => canExecute());

		/// <summary>
		/// Creates a relay command with specific action.
		/// </summary>
		/// <param name="execute">Action to execute.</param>
		/// <param name="canExecute">Method that determines if the command can execute.</param>
		public RelayCommand(Action<object> execute, Func<bool> canExecute) : this(execute)
			=> _canExecute = canExecute == null ? null : new Func<object, bool>(obj => canExecute());

		/// <summary>
		/// Creates a relay command with specific action.
		/// </summary>
		/// <param name="execute">Action to execute.</param>
		/// <param name="canExecute">Method that determines if the command can execute.</param>
		public RelayCommand(Action execute, Func<object, bool> canExecute) : this(execute)
			=> _canExecute = canExecute;

		/// <summary>
		/// Creates a relay command with specific action.
		/// </summary>
		/// <param name="execute">Action to execute.</param>
		/// <param name="canExecute">Method that determines if the command can execute.</param>
		public RelayCommand(Action<object> execute, Func<object, bool> canExecute) : this(execute)
			=> _canExecute = canExecute;

		#endregion

		#region ICommand Implementation

		/// <inheritdoc/>
		public event EventHandler CanExecuteChanged;

		/// <inheritdoc/>
		public bool CanExecute(object parameter)
		{
			var previousCanExecuteValue = _canExecuteValue;

			_canExecuteValue = _canExecute?.Invoke(parameter) != false;

			if (previousCanExecuteValue != _canExecuteValue)
			{
				CanExecuteChanged?.Invoke(this, EventArgs.Empty);
			}

			return _canExecuteValue;
		}

		/// <inheritdoc/>
		public void Execute(object parameter) => _execute?.Invoke(parameter);

		#endregion
	}

	/// <summary>
	/// Generic version of <see cref="RelayCommand"/> that
	/// converts the parameter into the given type.
	/// </summary>
	/// <typeparam name="T">Type used for the parameter.</typeparam>
	public class RelayCommand<T> : RelayCommand
	{
		/// <summary>
		/// Creates an instance of <see cref="RelayCommand"/> that can
		/// execute an action with parameter of type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="execute">Method to execute.</param>
		public RelayCommand(Action<T> execute) : base(param =>
		{
			// Convert the parameter
			var converted = param is string str && typeof(T) != typeof(string)
				? str.Convert<T>()
				: (T)param;

			// Execute the command
			execute(converted);
		})
		{ }

		/// <summary>
		/// Creates an instance of <see cref="RelayCommand"/> that can
		/// execute an action with parameter of type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="execute">Method to execute.</param>
		/// <param name="canExecute">Method that determines if the command can execute.</param>
		public RelayCommand(Action<T> execute, Func<T, bool> canExecute) : base(param =>
		{
			// Convert the parameter
			var converted = param is string str && typeof(T) != typeof(string)
				? str.Convert<T>()
				: (T)param;

			// Execute the command
			execute(converted);
		}, param =>
		{
			// Convert the parameter
			var converted = param is string str && typeof(T) != typeof(string)
				? str.Convert<T>()
				: (T)param;

			// Check if the command can be executed
			return canExecute(converted);
		})
		{ }
	}
}
