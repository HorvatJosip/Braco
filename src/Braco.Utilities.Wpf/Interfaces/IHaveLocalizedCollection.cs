using Braco.Services.Abstractions;
using System.Collections;
using System.Collections.Generic;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Defines a localized collection and a fill method
	/// that will fill the collection every time the language changes.
	/// </summary>
	public interface IHaveLocalizedCollection
	{
		/// <summary>
		/// Collection that contains localized values.
		/// </summary>
		IEnumerable Collection { get; }

		/// <summary>
		/// Used for filling the <see cref="Collection"/> with localized
		/// values using the <paramref name="localizer"/>.
		/// </summary>
		/// <param name="localizer">Localizer to use for filling the <see cref="Collection"/>
		/// with localized values.</param>
		void Fill(ILocalizer localizer);
	}

	/// <summary>
	/// Defines a localized collection and a fill method
	/// that will fill the collection every time the language changes.
	/// </summary>
	/// <typeparam name="T">Type used for the collection.</typeparam>
	public interface IHaveLocalizedCollection<T> : IHaveLocalizedCollection
	{
		/// <summary>
		/// Collection that contains localized values.
		/// </summary>
		new IEnumerable<T> Collection { get; }
	}
}
