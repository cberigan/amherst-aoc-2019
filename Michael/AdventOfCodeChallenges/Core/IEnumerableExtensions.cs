using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Returns non-overlapping windows of the source.
        /// </summary>
        public static IEnumerable<IList<T>> Split<T>(this IEnumerable<T> source, int count)
        {
            var l = new List<T>();
            int i = 0;
            foreach (var item in source)
            {
                if (i == count)
                {
                    yield return new List<T>(l);
                    l.Clear();
                    i = 0;
                }
                l.Add(item);
                i++;
            }
            yield return l;
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
            l.Add(acc);
            while (enu.MoveNext())
            {
                if (split(acc, enu.Current))
                {
                    yield return new List<T>(l);
                    l.Clear();
                }
                else
                    l.Add(acc);
                acc = enu.Current;
            }
            yield return l;
        }

        public static IEnumerable<List<T>> Split<T>(this IEnumerable<T> source, Func<T, T, IReadOnlyList<T>, bool> split)
        {
            var l = new List<T>();

            var enu = source.GetEnumerator();
            if (!enu.MoveNext()) yield break;

            T acc = enu.Current;
            l.Add(acc);
            while (enu.MoveNext())
            {
                if (split(acc, enu.Current, l))
                {
                    yield return new List<T>(l);
                    l.Clear();
                }
                
                l.Add(enu.Current);
                acc = enu.Current;
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
