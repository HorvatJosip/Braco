using AutoMapper;
using System.Threading;

namespace Braco.Utilities
{
	/// <summary>
	/// Model used for info box.
	/// </summary>
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	[AutoMap(typeof(InfoBoxContent))]
    public class InfoBoxContent
    {
		/// <summary>
		/// Title of the info box.
		/// </summary>
        public string Title { get; set; }

		/// <summary>
		/// Message to display inside the info box.
		/// </summary>
        public string Message { get; set; }

		/// <summary>
		/// Type of the info box.
		/// </summary>
        public InfoBoxType Type { get; set; }

		/// <summary>
		/// How long should the info box be displayed?
		/// </summary>
        public int Duration { get; set; } = 10;

		/// <summary>
		/// Is the info box dismissed?
		/// </summary>
        public bool Dismissed { get; set; } = true;

		/// <summary>
		/// Cancellation token source.
		/// </summary>
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

		/// <summary>
		/// Cancellation token from <see cref="CancellationTokenSource"/>.
		/// </summary>
        public CancellationToken CancellationToken => CancellationTokenSource.Token;

		/// <summary>
		/// Default constructor.
		/// </summary>
        public InfoBoxContent() { }

		/// <summary>
		/// Creates the info box model with desired values.
		/// </summary>
		/// <param name="title">title of the info box.</param>
		/// <param name="message">message to display inside the info box.</param>
		/// <param name="type">type of the info box.</param>
		/// <param name="duration">how long should the info box be displayed?</param>
		/// <param name="dismissed">is the info box dismissed?</param>
		public InfoBoxContent(string title, string message, InfoBoxType type, int duration = 10, bool dismissed = false)
        {
            Title = title;
            Message = message;
            Type = type;
            Duration = duration;
            Dismissed = dismissed;
        }
    }
}
