using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Extensions
{
    public static class EnumUtilities
    {
        public static TEnum ParseEnum<TEnum>(string s, TEnum defaultValue = default(TEnum))
            where TEnum : struct, Enum
        {
            if (string.IsNullOrEmpty(s))
                return defaultValue;

            return !Enum.TryParse(s, true, out TEnum value) ? defaultValue : value;
        }
    }
}
