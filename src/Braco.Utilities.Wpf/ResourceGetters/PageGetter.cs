using Braco.Services.Abstractions;
using System;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// <see cref="ResourceGetter"/> used for getting data
	/// about a page.
	/// </summary>
	public class PageGetter : ResourceGetter
	{
		/// <summary>
		/// Used for getting <see cref="PageViewModel"/> type for an identifier.
		/// </summary>
		/// <param name="identifier">Identifier from which to determine the type.</param>
		/// <returns>Type of page.</returns>
		public virtual Type GetPageType(string identifier)
			=> null;
	}
}
