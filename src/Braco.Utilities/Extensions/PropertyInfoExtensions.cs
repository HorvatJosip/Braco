using System.Linq;
using System.Reflection;

namespace Braco.Utilities.Extensions
{
	/// <summary>
	/// Extensions for <see cref="PropertyInfo"/> class.
	/// </summary>
	public static class PropertyInfoExtensions
	{
		/// <summary>
		/// Gets the backing field of the given <paramref name="property"/>.
		/// <para>Note: this should be used carefully (see <see cref="TypeExtensions.BackingFieldIndicator"/>).</para>
		/// </summary>
		/// <param name="property">Property whose backing field should be retrieved.</param>
		/// <returns>Backing field of the given <paramref name="property"/>.</returns>
		public static FieldInfo GetBackingField(this PropertyInfo property)
			=> property.DeclaringType
				.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
				.FirstOrDefault(field => field.Name.Contains(property.Name) && field.Name.Contains(TypeExtensions.BackingFieldIndicator));
	}
}
