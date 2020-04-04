using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeChallenges.Core
{
    public readonly struct IndexableNumber
    {
        private readonly int _i;

        public IndexableNumber(int i) => _i = i;

        public int Length => (int)Math.Floor(Math.Log10(_i) + 1);

        /// <summary>
        /// Indexes start at 1!
        /// </summary>
        public int this[int index] => _i / (int)Math.Pow(10, Length - index) % 10;

        public IEnumerable<int> DigitsBase10()
        {
            for (int i = 1; i < Length + 1; i++)
            {
                yield return this[i];
            }
        }
    }
}
