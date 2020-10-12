namespace Braco.Services.Abstractions
{
    /// <summary>
    /// Arguments for the <see cref="IConfigurationService.SettingChanged"/> event.
    /// </summary>
    public class SettingChangedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Setting that changed.
        /// </summary>
        public string Setting { get; }

        /// <summary>
        /// Old value of the setting.
        /// </summary>
        public object OldValue { get; }

        /// <summary>
        /// New value of the setting.
        /// </summary>
        public object NewValue { get; }

		/// <summary>
		/// Creates an instance of the arguments.
		/// </summary>
		/// <param name="setting">Setting that changed.</param>
		/// <param name="oldValue">Old value of the setting.</param>
		/// <param name="newValue">New value of the setting.</param>
		public SettingChangedEventArgs(string setting, object oldValue, object newValue)
        {
            Setting = setting;
            OldValue = oldValue;
            NewValue = newValue;
		}

		/// <inheritdoc/>
		public override string ToString()
			=> $"{Setting}: {OldValue} -> {NewValue}";
	}
}
