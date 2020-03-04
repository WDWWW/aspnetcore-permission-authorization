using System.Collections.Generic;

namespace Wd3w.AspNetCore.Permission.Internal
{
    internal static class StringHelper
    {
        public static string JoinAsString(this IEnumerable<string> strings, string seperator)
        {
            return string.Join(seperator, strings);
        }

        public static string Wrap(this string str, string left, string right = null)
        {
            return left + str + (right ?? left);
        }
    }
}