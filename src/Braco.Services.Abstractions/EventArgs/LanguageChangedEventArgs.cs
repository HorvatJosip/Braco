namespace Braco.Services.Abstractions
{
    /// <summary>
    /// Arguments for the <see cref="ILocalizer.LanguageChanged"/> event.
    /// </summary>
    public class LanguageChangedEventArgs : System.EventArgs
    {
		/// <summary>
		/// Culture to which we changed.
		/// </summary>
        public string Culture { get; }

		/// <summary>
		/// Creates an instance of the arguments.
		/// </summary>
		/// <param name="culture">Culture to which we changed.</param>
		public LanguageChangedEventArgs(string culture)
        {
            Culture = culture;
        }
    }
}
