namespace Braco.Utilities
{
	/// <summary>
	/// Holds information about an enum value.
	/// </summary>
    public class EnumItem
    {
		/// <summary>
		/// Localized version of the enum.
		/// </summary>
        public string LocalizedString { get; set; }

		/// <summary>
		/// Value of the enum.
		/// </summary>
        public int Value { get; set; }

		/// <inheritdoc/>
        public override string ToString() => $"[{Value}] {LocalizedString}";
		/// <inheritdoc/>
        public override int GetHashCode() => new { Value }.GetHashCode();
		/// <inheritdoc/>
        public override bool Equals(object obj) => obj is EnumItem other && Value == other.Value;
		/// <inheritdoc/>
        public static bool operator ==(EnumItem left, EnumItem right) => Equals(left, right);
		/// <inheritdoc/>
        public static bool operator !=(EnumItem left, EnumItem right) => !Equals(left, right);
    }
}
