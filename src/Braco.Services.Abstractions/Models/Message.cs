using System.Linq;

namespace Braco.Services.Abstractions
{
	/// <summary>
	/// Message that contains a specific type and content.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// An error occurred.
		/// </summary>
		public const string Error = nameof(Error);

		/// <summary>
		/// Something that didn't make the request end, but should be avioded.
		/// </summary>
		public const string Warning = nameof(Warning);

		/// <summary>
		/// Simply an informational message.
		/// </summary>
		public const string Information = nameof(Information);

		private readonly string[] _predefinedTypes = { Error, Warning, Information };

		/// <summary>
		/// Type of message.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Defines if <see cref="Type"/> is custom (isn't <see cref="Error"/>,
		/// <see cref="Warning"/> or <see cref="Information"/>).
		/// </summary>
		public bool IsCustom => !_predefinedTypes.Contains(Type);

		/// <summary>
		/// Content of the message.
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Creates a message with given type and content.
		/// </summary>
		/// <param name="type">Type of message.</param>
		/// <param name="content">Content of the message.</param>
		public Message(string type, string content)
		{
			Type = type;
			Content = content;
		}

		/// <summary>
		/// Generates an instance that has <see cref="Type"/>
		/// set to <see cref="Error"/>.
		/// </summary>
		/// <param name="content">Content of the message-</param>
		/// <returns><see cref="Message"/> instance.</returns>
		public static Message FromError(string content)
			=> new Message(Error, content);

		/// <summary>
		/// Generates an instance that has <see cref="Type"/>
		/// set to <see cref="Warning"/>.
		/// </summary>
		/// <param name="content">Content of the message-</param>
		/// <returns><see cref="Message"/> instance.</returns>
		public static Message FromWarning(string content)
			=> new Message(Warning, content);

		/// <summary>
		/// Generates an instance that has <see cref="Type"/>
		/// set to <see cref="Information"/>.
		/// </summary>
		/// <param name="content">Content of the message-</param>
		/// <returns><see cref="Message"/> instance.</returns>
		public static Message FromInformation(string content)
			=> new Message(Information, content);
	}
}
