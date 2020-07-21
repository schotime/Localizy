using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Localizy
{
    public class StringToken
    {
        private readonly string _defaultValue;
        private string _key;
        private readonly Lazy<LocalizationKey> _localizationKey;

        private static readonly ConcurrentBag<Type> _latchedTypes = new ConcurrentBag<Type>();
        private Type _type;

        protected static void fillKeysOnFields(Type tokenType)
        {
            if (_latchedTypes.Contains(tokenType)) return;

            tokenType.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.FieldType.CanBeCastTo<StringToken>())
                .Each(field =>
                {
                    var token = field.GetValue(null).As<StringToken>();
                    if (token._key == null) // leave it checking the field, unless you just really enjoy stack overflow exceptions
                    {
                        token.Key = field.Name;
                    }
                });
                

            _latchedTypes.Add(tokenType);
        }

        protected StringToken(string defaultValue) : this(null, defaultValue)
        {
        }

        protected StringToken(string key, string defaultValue, string localizationNamespace = null, bool namespaceByType = true)
        {
            _key = key;
            _defaultValue = defaultValue;
            _type = GetContainerType();
            _localizationKey = new Lazy<LocalizationKey>(() => BuildKey(_type, localizationNamespace, namespaceByType));
        }

        protected virtual Type GetContainerType()
        {
            return GetType();
        }

        public virtual string Key
        {
            get
            {
                if (_key.IsEmpty())
                {
                    fillKeysOnFields(GetContainerType());
                }

                ThrowIfKeyNotSet();
                
                return _key;
            }
            protected set { _key = value; }
        }

        private void ThrowIfKeyNotSet()
        {
            if (_key == null)
            {
                throw new Exception("Key is not set. This usually means the Type provided to the StringToken<T> class is not the parent class");
            }
        }

        public Type ContainedType
        {
            get { return _type; }
        }

        public virtual string FullKey
        {
            get { return _localizationKey.Value.ToString(); }
        }

        public string DefaultValue
        {
            get { return _defaultValue; }
        }

        public static StringToken FromKeyString(string key)
        {
            return new StringToken(key, null, namespaceByType: false);
        }

        public static StringToken FromKeyString(string key, string defaultValue)
        {
            return new StringToken(key, defaultValue, namespaceByType: false);
        }

        public static StringToken FromType<T>()
        {
            return FromType(typeof (T));
        }

        public static StringToken FromType(Type type)
        {
            return new StringToken(type.Name, type.Name, namespaceByType: false);
        }

        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        ///   Conditionally render the string based on a condition. Convenient if you want to avoid a bunch of messy script tags in the views.
        /// </summary>
        /// <param name = "condition"></param>
        /// <returns></returns>
        public string ToString(bool condition)
        {
            return condition ? ToString(null) : string.Empty;
        }

        public string ToString(CultureInfo culture)
        {
            return LocalizationManager.GetText(this, culture);
        }

        static StringToken()
        {
            LocalizationManager = new LocalizationManagerStaticAdapter();
        }

        public static ILocalizationManager LocalizationManager { get; set; }

        public string ToFormat(params object[] args)
        {
            return string.Format(ToString(), args);
        }

        public bool Equals(StringToken obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj.ToLocalizationKey().ToString(), ToLocalizationKey().ToString());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is StringToken) return Equals((StringToken) obj);

            return false;
        }

        public override int GetHashCode()
        {
            return ToLocalizationKey().ToString().GetHashCode();
        }

        public static implicit operator string(StringToken token)
        {
            return token.ToString();
        }

        protected virtual LocalizationKey BuildKey(Type type, string localizationNamespace, bool namespaceByType)
        {
            var localizationNs = localizationNamespace ?? (namespaceByType ? string.Join(".", GetTypeHierarchy(type).Select(x => x.Name)) : null);
            if (_key == null)
            {
                fillKeysOnFields(type);
            }

            ThrowIfKeyNotSet();

            return localizationNs.IsNotEmpty()
                ? new LocalizationKey(_key, localizationNs) 
                : new LocalizationKey(_key);
        }

        private static IEnumerable<Type> GetTypeHierarchy(Type type)
        {
            var types = new List<Type> { type };
            while (type.DeclaringType != null)
            {
                types.Add(type.DeclaringType);
                type = type.DeclaringType;
            }

            types.Reverse();
            return types;
        }

        public LocalizationKey ToLocalizationKey()
        {
            return _localizationKey.Value;
        }

        public static StringToken Find(Type tokenType, string key)
        {
            var fields = tokenType.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.FieldType.CanBeCastTo<StringToken>());

            foreach (var fieldInfo in fields)
            {
                var token = fieldInfo.GetValue(null).As<StringToken>();
                if (token.Key == key)
                {
                    return token;
                }
            }

            return null;
        }

        public static StringToken Find<T>(string key) where T : StringToken
        {
            return Find(typeof (T), key);
        }
    }
}