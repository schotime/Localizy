using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Localizy
{
    public class LocalizationKey
    {
        private readonly string _key;
        private readonly string _namespace;

        public LocalizationKey(string key)
            : this(NormalizeKey(key))
        {
        }

        public LocalizationKey(LocalizationKey localizationKey)
        {
            _key = localizationKey.Key;
            _namespace = localizationKey.Namespace;
        }

        public LocalizationKey(string key, string ns)
        {
            _key = key;
            _namespace = ns ?? string.Empty;
        }

        public LocalizationKey(PropertyInfo property)
        {
            _key = property.Name;
            _namespace = property.DeclaringType.FullName;
        }

        public bool Equals(LocalizationKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(_key, other._key, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(_namespace, other._namespace, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(LocalizationKey)) return false;
            return Equals((LocalizationKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_key.ToLower().GetHashCode() * 397) ^ _namespace.ToLower().GetHashCode();
            }
        }

        public override string ToString()
        {
            return _namespace.IsEmpty()
                ? _key
                : _namespace + ":" + _key;
        }

        public static implicit operator LocalizationKey(string key)
        {
            return new LocalizationKey(key);
        }

        private static LocalizationKey NormalizeKey(string key)
        {
            var keyParts = key.Split(':', '.');
            if (keyParts.Length == 1)
            {
                return new LocalizationKey(keyParts[0], string.Empty);
            }

            return new LocalizationKey(keyParts[keyParts.Length - 1], string.Join(".", keyParts.Take(keyParts.Length - 1)));
        }

        public string Key { get { return _key; } }
        public string Namespace { get { return _namespace; } }
    }
}