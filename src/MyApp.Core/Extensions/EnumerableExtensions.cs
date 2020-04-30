using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T>? list)
        {
            return list ?? Enumerable.Empty<T>();
        }

        public static string StringJoin<T>(this IEnumerable<T>? list, string separator = ", ")
        {
            return string.Join(separator, list.OrEmptyIfNull());
        }
    }
}
