using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCodeChallenges.C1;
using MoreLinq;

namespace AdventOfCodeChallenges.C2
{
    public sealed class Challenge
    {
        private readonly C1.Challenge _c1;

        public Challenge()
        {
            _c1 = new C1.Challenge();
        }

        public long Run(long[] input = null) =>
            (input ?? C1.Challenge.Inputs)
                .SelectMany(i => MoreEnumerable.Unfold(i, x => _c1.FuelRequired(x), x => x > 0, x => x, x => x))
                .Sum();
    }

}
