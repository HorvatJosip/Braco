using System;

namespace Braco.Utilities
{
	/// <summary>
	/// Utilities for number types such as <see cref="int"/>,
	/// <see cref="double"/>, <see cref="decimal"/>.
	/// </summary>
	public static class NumberUtilities
    {
		/// <summary>
		/// Random number generator.
		/// </summary>
        public static Random Rng { get; } = new Random();

		/// <summary>
		/// Does a random roll and checks if it succeeded.
		/// </summary>
		/// <param name="percentage">Percentage to win the roll.</param>
		/// <returns>True if rolled below or equal to the percentage,
		/// otherwise false.</returns>
        public static bool PercentageRoll(double percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage));

            return Rng.Next(101) <= percentage;
        }
    }
}
