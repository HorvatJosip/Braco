using Braco.Utilities.Extensions;
using System;
using System.ComponentModel;

namespace Braco.Services
{
    /// <summary>
    /// Represents an item in the configuration.
    /// </summary>
    public class ConfigurationItem
    {
        private const string typeSeparator = "$$__$$TYPE$$__$$";
        private const string valueSeparator = "_\\|/_";

        private object value;
        private TypeConverter converter;

		/// <summary>
		/// Key of the item.
		/// </summary>
        public string Key { get; }

		/// <summary>
		/// Type of the item.
		/// </summary>
        public Type Type { get; private set; }

		/// <summary>
		/// Value of the item.
		/// </summary>
        public object Value
        {
            get => value;
            set
            {
                if (!Equals(value, this.value))
                {
                    this.value = value;

                    SetTypeAndConverter(value?.GetType());
                }
            }
        }

		/// <summary>
		/// Creates a configuration item.
		/// </summary>
		/// <param name="key">Key of the item.</param>
		/// <param name="type">Type of the item.</param>
        public ConfigurationItem(string key, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (key.IsNullOrWhiteSpace()) throw new ArgumentException("You must provide a key", nameof(key));

            Key = key;

            SetTypeAndConverter(type);
        }

		/// <summary>
		/// Creates a configuration item.
		/// </summary>
		/// <param name="key">Key of the item.</param>
		/// <param name="value">Value of the item.</param>
		public ConfigurationItem(string key, object value)
        {
            if (key.IsNullOrWhiteSpace()) throw new ArgumentException("You must provide a key", nameof(key));

            Key = key;
            Value = value;

            if (Type == null) throw new ArgumentException("Couldn't set the type from the given value", nameof(value));
        }

		/// <summary>
		/// Creates a configuration item.
		/// </summary>
		/// <param name="key">Key of the item.</param>
		/// <param name="value">Value of the item.</param>
		/// <param name="type">Type of the item.</param>
		public ConfigurationItem(string key, string value, Type type) : this(key, type)
        {
            this.value = converter.ConvertFromInvariantString(value);
        }

        private void SetTypeAndConverter(Type type)
        {
            if (type != null)
            {
                Type = type;
                converter = TypeDescriptor.GetConverter(type);
            }
        }

		/// <summary>
		/// Gets string representation of the item.
		/// </summary>
		/// <returns>String representation of the item.</returns>
		public string StringValue() => converter?.ConvertToInvariantString(value);

		/// <summary>
		/// Tries to convert the value to specified type.
		/// </summary>
		/// <typeparam name="T">Type of value to get.</typeparam>
		/// <returns>Value converted to type <typeparamref name="T"/> or
		/// default value if conversion didn't succeed.</returns>
		public T GetValue<T>()
		{
			if (value == null) return default;

			var type = typeof(T);

			if (type == Type) return (T)value;

			if(value is string str) return str.Convert<T>();

			var stringValue = StringValue();

			return stringValue == null ? default : stringValue.Convert<T>();
		}

		/// <summary>
		/// Converts the item into a line for storing it.
		/// </summary>
		/// <returns>Item converted into a line format.</returns>
        public string ConvertToLine() => string.Format("{0}{1}{2}{3}{4}",
            Key,
            valueSeparator,
            StringValue(),
            typeSeparator,
            Type.AssemblyQualifiedName
        );

		/// <inheritdoc/>
        public override string ToString() => $"[{Key}] {Value} (Type: {Type})";
		/// <inheritdoc/>
        public override bool Equals(object obj) => obj is ConfigurationItem configuration && Key == configuration.Key;
		/// <inheritdoc/>
        public override int GetHashCode() => new { Key }.GetHashCode();

		/// <summary>
		/// Creates a configuration item from stored line.
		/// </summary>
		/// <param name="line">Line that was created using <see cref="ConvertToLine"/>.</param>
		/// <returns>instance of the item if it is valid, otherwise null.</returns>
        public static ConfigurationItem CreateFromLine(string line)
        {
            var parts = line.Split(valueSeparator);
            var key = parts[0];

            var valueInfo = parts[1].Split(typeSeparator);

            var type = Type.GetType(valueInfo[1]);

            return type == null
                ? null
                : new ConfigurationItem(key, valueInfo[0], type);
        }
    }
}
