using System.Linq;
using MoreLinq;

namespace AdventOfCodeChallenges.C1
{
    public sealed class ChallengePt2
    {
        private readonly C1.Challenge _c1;

        public ChallengePt2()
        {
            _c1 = new C1.Challenge();
        }

        public long Run(long[] input = null) =>
            (input ?? C1.Challenge.Inputs)
                .SelectMany(i => MoreEnumerable.Unfold(i, x => _c1.FuelRequired(x), x => x > 0, x => x, x => x))
                .Sum();
    }

}
