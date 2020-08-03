using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCodeChallenges.Core
{
    public static class Comparer
    {
        public static IComparer<T> Create<T, TB>(Func<T, TB> selector)
            where TB : IComparable {
            return new ComparerImpl<T, TB>(selector);
        }

        private class ComparerImpl<T, TB> : IComparer<T> where TB : IComparable
        {
            private Func<T, TB> selector;

            public ComparerImpl(Func<T, TB> selector)
            {
                this.selector = selector;
            }

            public int Compare([AllowNull] T x, [AllowNull] T y) => 
                selector(x).CompareTo(selector(y));
        }

    }
}
