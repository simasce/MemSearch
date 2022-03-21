using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{
    public static class Extensions
    {
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static string RemoveFromStringIfExists(this string str, string delimiter)
        {
            int idx = str.IndexOf(delimiter);
            if (idx == -1)
                return str;
            return str.Substring(0, idx);
        }

        public static string RemoveFromStringIfExistsIgnoreString(this string str, string delimiter)
        {
            int idx = str.IndexOf(delimiter);
            if (idx == -1)
                return str;
            if (str.Trim().EndsWith('"'))
                return str;
            return str.Substring(0, idx);
        }
    }
}
