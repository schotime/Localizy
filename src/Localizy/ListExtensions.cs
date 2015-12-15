using System;
using System.Collections.Generic;
using System.Linq;

namespace Localizy
{
    internal static class ListExtensions
    {
        public static void Each<T>(this IEnumerable<T> values, Action<T> eachAction)
        {
            foreach (T obj in values)
            {
                eachAction(obj);
            }
        }
    }
}