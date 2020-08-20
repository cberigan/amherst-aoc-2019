using System;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.Core
{
    public readonly struct IndexableNumber
    {
        private readonly int _i;

        public IndexableNumber(int i) => _i = i;

        public int Length => (int)Math.Floor(Math.Log10(_i) + 1);

        /// <summary>
        /// Indexes start at 0! Does not return the sign. Always returns a positive number.
        /// </summary>
        public int this[int index] => Math.Abs((_i / (int)Math.Pow(10, index)) % 10);

        public IEnumerable<int> DigitsBase10()
        {
            for (int i = 1; i < Length + 1; i++)
            {
                yield return this[i];
            }
        }
    }
}
