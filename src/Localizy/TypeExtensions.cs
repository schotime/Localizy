using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Localizy
{
    public static class TypeExtensions
    {
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }

        public static IEnumerable<Type> RecurseNestedTypes(this Type type)
        {
            yield return type;

            var stack = new Stack<Type>(new[] { type });

            do
            {
                var currentType = stack.Pop();
                var nestedTypes = currentType.GetTypeInfo().GetNestedTypes(BindingFlags.Public);

                foreach (var nestedType in nestedTypes)
                {
                    yield return nestedType;
                }

                foreach (var nestedType in nestedTypes)
                {
                    stack.Push(nestedType);
                }
            }
            while (stack.Count > 0);
        }

        /// <summary>
        /// Does a hard cast of the object to T.  *Will* throw InvalidCastException
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T As<T>(this object target)
        {
            return (T)target;
        }
        
        public static bool CanBeCastTo<T>(this Type type)
        {
            if (type == null) return false;
            Type destinationType = typeof(T);

            return CanBeCastTo(type, destinationType);
        }

        public static bool CanBeCastTo(this Type type, Type destinationType)
        {
            if (type == null) return false;
            if (type == destinationType) return true;

            return destinationType.GetTypeInfo().IsAssignableFrom(type);
        }
    }
}
