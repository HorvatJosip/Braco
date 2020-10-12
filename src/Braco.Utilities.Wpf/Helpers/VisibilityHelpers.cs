using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Helpers used for converting to <see cref="Visibility"/>.
	/// </summary>
	public static class VisibilityHelpers
	{
		/// <summary>
		/// Constant to use for inverting the logic.
		/// </summary>
		public const string InvertParam = "invert";
		/// <summary>
		/// Constant to use if <see cref="Visibility.Hidden"/>
		/// should be used instead of <see cref="Visibility.Collapsed"/>.
		/// </summary>
		public const string HideParam = "hide";
		/// <summary>
		/// Constant to use if <see cref="Visibility.Hidden"/>
		/// should be used instead of <see cref="Visibility.Collapsed"/>
		/// and the logic should be inverted.
		/// </summary>
		public const string HideAndInvertParam = "hide and invert";

		/// <summary>
		/// Converts to <see cref="Visibility"/> based on given values.
		/// </summary>
		/// <param name="visible">Should <see cref="Visibility.Visible"/> be the result?</param>
		/// <param name="param">Logic alteration parameter. Can be
		/// <see cref="InvertParam"/>, <see cref="HideParam"/>, <see cref="HideAndInvertParam"/>
		/// or anything else to not alter the logic.</param>
		/// <returns></returns>
		public static Visibility Convert(bool visible, object param)
			// Determine what to return based on the values
			=> param?.ToString().ToLower() switch
			{
				HideParam => visible ? Visibility.Visible : Visibility.Hidden,

				HideAndInvertParam => visible ? Visibility.Hidden : Visibility.Visible,

				InvertParam => visible ? Visibility.Collapsed : Visibility.Visible,

				_ => visible ? Visibility.Visible : Visibility.Collapsed,
			};
	}
}
