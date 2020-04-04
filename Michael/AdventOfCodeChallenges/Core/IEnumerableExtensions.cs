using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeChallenges.Core
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T, T)> Window2<T>(this IEnumerable<T> source)
        {
            var enu = source.GetEnumerator();
            enu.MoveNext();
            var first = enu.Current;
            while (enu.MoveNext())
            {
                yield return (first, enu.Current);
                first = enu.Current;
            }
        }

        public static IEnumerable<List<T>> NotStupidSplit<T>(this IEnumerable<T> source, Func<T, bool> split)
        {
            var l = new List<T>();
            foreach (var item in source)
            {
                if (split(item))
                {
                    yield return new List<T>(l);
                    l.Clear();
                }
                l.Add(item);
            }
            yield return l;
        }
    }
}
