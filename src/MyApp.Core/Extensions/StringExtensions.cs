using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            return string.IsNullOrEmpty(value) ? value : new string(value.Take(maxLength).ToArray());
        }
    }
}
