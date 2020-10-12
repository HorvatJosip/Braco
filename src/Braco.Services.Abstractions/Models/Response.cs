using System.Collections.Generic;
using System.Linq;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Represents a response from a service.
	/// </summary>
	public abstract class Response<T> where T : Response<T>, new()
	{
		/// <summary>
		/// Request was cancelled.
		/// </summary>
		public const string RequestCancelled = "Cancelled";

		/// <summary>
		/// Response messages.
		/// </summary>
		public ICollection<Message> Messages { get; set; }

		/// <summary>
		/// Flag that indicates if the request finished or not.
		/// </summary>
		public bool Finished { get; private set; } = true;

		/// <summary>
		/// Reason why the request didn't finish.
		/// </summary>
		public string UnfinishedReason { get; private set; }

		/// <summary>
		/// Checks if there are any messages with type <paramref name="messageType"/>.
		/// </summary>
		/// <param name="messageType">Type of message to find.</param>
		/// <returns>If there are any messages with given type.</returns>
		public bool HasSpecificTypeOfMessage(string messageType)
			=> Messages?.Any(message => message.Type == messageType) ?? false;

		/// <summary>
		/// Checks if there are any messages with type <see cref="Message.Error"/>.
		/// </summary>
		/// <returns>If there are any messages with type <see cref="Message.Error"/>.</returns>
		public bool HasErrors()
			=> HasSpecificTypeOfMessage(Message.Error);

		/// <summary>
		/// Creates a response with <see cref="Finished"/> set to false.
		/// </summary>
		/// <typeparam name="T">Type of response to create.</typeparam>
		/// <param name="unfinishedReason">Reason why the request didn't finish.</param>
		/// <param name="messages">Messages to add to the response.</param>
		/// <returns>Instance of the response that is unfinished.</returns>
		public static T Unfinished(string unfinishedReason, params Message[] messages)
			=> new T
			{
				UnfinishedReason = unfinishedReason,
				Finished = false,
				Messages = new List<Message>(messages)
			};

		/// <summary>
		/// The request didn't finish because it was cancelled
		/// (this will have <see cref="UnfinishedReason"/> as <see cref="RequestCancelled"/>).
		/// </summary>
		/// <typeparam name="T">Type of response to create.</typeparam>
		/// <param name="messages">Messages to add to the response.</param>
		/// <returns>Instance of the response that is unfinished because it was cancelled.</returns>
		public static T Cancelled(params Message[] messages)
			=> Unfinished(RequestCancelled, messages);

		/// <summary>
		/// Creates a response with <see cref="Finished"/> set to false
		/// and messages that have type <see cref="Message.Error"/>.
		/// </summary>
		/// <typeparam name="T">Type of response to create.</typeparam>
		/// <param name="unfinishedReason">Reason why the request didn't finish.</param>
		/// <param name="errors">Error messages to add to the response.</param>
		/// <returns>Instance of the response that is unfinished.</returns>
		public static T FromErrors(string unfinishedReason, params string[] errors)
			=> Unfinished(unfinishedReason, errors.Select(error => Message.FromError(error)).ToArray());
	}
}
