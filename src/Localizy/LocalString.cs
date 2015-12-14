using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Localizy
{
    public class LocalString : IComparable<LocalString>, IComparable
    {
        public static LocalString ReadFrom(string line)
        {
            var parts = line.Trim().Split('=');
            if (parts.Length != 2)
            {
                throw new ArgumentException(string.Format("LocalString must be expressed as 'value=display', '{0}' is invalid", line));
            }

            return new LocalString(parts.First(), parts.Last());
        }

        public static IEnumerable<LocalString> ReadAllFrom(string text)
        {
            return text.ReadLines()
                .Select(x => x.Trim())
                .Where(x => x.IsNotEmpty())
                .Select(ReadFrom);
        }

        public LocalString(string key)
        {
            Key = key;
            Display = key;
        }

        public LocalString(string key, string display)
        {
            Key = key;
            Display = display;
        }

        public LocalString()
        {
        }

        public LocalString(Type type)
            : this(type.Name)
        {

        }

        public LocalString(StringToken token)
        {
            Key = token.ToLocalizationKey().ToString();
            Display = token.ToString();
        }

        public string Key { get; set; }

        public string Display { get; set; }

        public bool Equals(LocalString obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.Key, Key) && Equals(obj.Display, Display);
        }

        public int CompareTo(LocalString other)
        {
            if (other == null) return 1;
            if (other.Display == null && Display == null) return 0;
            if (Display == null) return -1;

            return Display.CompareTo(other.Display);
        }

        public int CompareTo(object obj)
        {
            return Display.CompareTo(((LocalString)obj).Display);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(LocalString)) return false;
            return Equals((LocalString)obj);
        }

        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", Key, Display);
        }
    }
}