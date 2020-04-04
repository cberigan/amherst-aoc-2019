using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeChallenges.C4
{
    public static class Challenge
    {
        //It is a six-digit number.
        //The value is within the range given in your puzzle input.
        //Two adjacent digits are the same (like 22 in 122345).
        //Going from left to right, the digits never decrease; they only ever increase or stay the same(like 111123 or 135679).
        public static readonly Range Range = 172851..675869;

        public class Pt1 {
            public int Run(Range? range = null)
            {
                var rr = range ?? Range;
                return Enumerable.Range(rr.Start.Value, rr.End.Value - rr.Start.Value)
                    .Where(HasConsecutiveDigits)
                    .Where(HasNoDecreasingDigits)
                    .Count();
            }
        
        }

        public class Pt2 {
            public int Run(Range? range = null)
            {
                var rr = range ?? Range;
                return Enumerable.Range(rr.Start.Value, rr.End.Value - rr.Start.Value)
                    .Where(HasConsecutiveDigits)
                    .Where(HasNoDecreasingDigits)
                    .Where(AnExplicitPairExists)
                    .Count();
            }
        }

        private static bool HasConsecutiveDigits(int i) =>
            new IndexableNumber(i).DigitsBase10().Window2().Any(t => t.Item1 == t.Item2);

        private static bool HasNoDecreasingDigits(int i) =>
            new IndexableNumber(i).DigitsBase10().Window2().All(t => t.Item1 <= t.Item2);

        private static bool AnExplicitPairExists(int i)
        {
            int last = new IndexableNumber(i)[1];
            var parts = new IndexableNumber(i).DigitsBase10().NotStupidSplit(x =>
            {
                // split on change
                bool flag = x != last;
                last = x;
                return flag;
            });
            var distinctParts = parts.DistinctBy(p => p.Aggregate(0, (acc, curr) => acc * 497 + curr));
            return distinctParts.Any(p => p.Count() == 2);
        }
    }
}
