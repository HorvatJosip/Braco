namespace Braco.Utilities.Wpf.Controls
{
	/// <summary>
	/// Extensions for quicker checks with <see cref="Spaced"/> enum.
	/// </summary>
	public static class SpacedExtensions
	{
		/// <summary>
		/// Checks if the value is either <see cref="Spaced.Between"/>
		/// or <see cref="Spaced.AroundAndBetween"/>.
		/// </summary>
		/// <param name="spaced">Enum value to check.</param>
		/// <returns>If the value is either <see cref="Spaced.Between"/>
		/// or <see cref="Spaced.AroundAndBetween"/>.</returns>
		public static bool Between(this Spaced spaced)
			=> spaced == Spaced.Between || spaced == Spaced.AroundAndBetween;

		/// <summary>
		/// Checks if the value is either <see cref="Spaced.Around"/>
		/// or <see cref="Spaced.AroundAndBetween"/>.
		/// </summary>
		/// <param name="spaced">Enum value to check.</param>
		/// <returns>If the value is either <see cref="Spaced.Around"/>
		/// or <see cref="Spaced.AroundAndBetween"/>.</returns>
		public static bool Around(this Spaced spaced)
			=> spaced == Spaced.Around || spaced == Spaced.AroundAndBetween;
	}
}
