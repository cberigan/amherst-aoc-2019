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

        public static IEnumerable<List<T>> Split<T>(this IEnumerable<T> source, Func<T, T, bool> split)
        {
            var l = new List<T>();

            var enu = source.GetEnumerator();
            if (!enu.MoveNext()) yield break;

            T acc = enu.Current;
            while (enu.MoveNext())
            {
                if (split(acc, enu.Current))
                {
                    yield return new List<T>(l);
                    l.Clear();
                }
            }
            yield return l;
        }

        /// <summary>
        /// Returns an enumeration of all values contained in a Range.
        /// </summary>
        public static IEnumerable<int> Sequence(this Range range) {
            (int start, int end) = (range.Start.Value, range.End.Value);
            return Enumerable.Range(start, end - start);
        }
    }
}
