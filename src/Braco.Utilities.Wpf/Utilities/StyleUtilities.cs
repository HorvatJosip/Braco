using Braco.Utilities.Extensions;
using System;
using System.Windows;

namespace Braco.Utilities.Wpf
{
	/// <summary>
	/// Utilities focused on WPF styles.
	/// </summary>
	public static class StyleUtilities
	{
		/// <summary>
		/// Overrides metadata for given style types.
		/// <para>You may want to define a style with target type <see cref="Window"/>
		/// and then use it by calling this method.</para>
		/// </summary>
		/// <param name="types">Types to override styles for.</param>
		public static void OverrideStyles(params Type[] types)
			=> types.ForEach(type => FrameworkElement.StyleProperty.OverrideMetadata
			(
				forType: type,
				typeMetadata: new FrameworkPropertyMetadata
				{
					DefaultValue = Application.Current.FindResource(type)
				}
			));
	}
}
