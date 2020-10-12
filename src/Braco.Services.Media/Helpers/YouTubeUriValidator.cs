using Braco.Utilities.Extensions;
using System;
using System.Linq;

namespace Braco.Services.Media
{
	/// <summary>
	/// Used for validating YouTube uris.
	/// </summary>
	public static class YouTubeUriValidator
	{
		/// <summary>
		/// Characters required to be in a YouTube uri.
		/// </summary>
		public const string RequiredHostChars = "youtube";

		/// <summary>
		/// Scheme separator of any uri.
		/// </summary>
		public const string UriSchemeSeparator = "://";

		/// <summary>
		/// www.
		/// </summary>
		public const string WWW = "www";

		/// <summary>
		/// Determines if the uri is in valid format.
		/// <para>If there is a minor tweak required, true will be returned and
		/// the uri given out as <paramref name="formatted"/> parameter.</para>
		/// </summary>
		/// <param name="uri">Uri to check if it is valid.</param>
		/// <param name="formatted">Tweaked uri to be correct.</param>
		/// <returns>If the given uri is in valid format or not.</returns>
		public static bool IsUriInValidFormat(string uri, out string formatted)
		{
			formatted = uri;

			if (formatted.IsNotNullOrWhiteSpace())
			{
				if (!formatted.ToLower().StartsWith(Uri.UriSchemeHttp))
				{
					formatted = $"{Uri.UriSchemeHttps}{UriSchemeSeparator}{formatted}";
				}

				if (!formatted.ToLower().Contains(WWW))
				{
					formatted = formatted.Insert(formatted.IndexOf(UriSchemeSeparator) + UriSchemeSeparator.Length, $"{WWW}.");
				}

				if (Uri.TryCreate(formatted, UriKind.Absolute, out var createdUri))
				{
					return
						(createdUri.Scheme == Uri.UriSchemeHttp ||
						createdUri.Scheme == Uri.UriSchemeHttps) &&
						RequiredHostChars.All(character => createdUri.Host.Contains(character));
				}
			}

			return false;
		}
	}
}
