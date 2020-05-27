using System;

namespace AdventOfCodeChallenges.Core
{
    public static class SpanExtensions
    {
        public static int CustomHashCode(this ReadOnlySpan<char> s)
        {
            int hash = 497;
            foreach (var c in s)
                hash = HashCode.Combine(hash, (int)c);
            return hash;
        }
    }
}
