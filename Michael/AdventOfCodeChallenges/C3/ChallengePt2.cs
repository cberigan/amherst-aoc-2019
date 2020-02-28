using System.Linq;

namespace AdventOfCodeChallenges.C3
{
    public class ChallengePt2
    {
        private readonly Challenge _c1;

        public ChallengePt2()
        {
            _c1 = new Challenge();
        }

        public int Run()
        {
            var originalInputs = Challenge.Inputs;
            var possibleAlterations = from i in Enumerable.Range(0, 99)
                                      from j in Enumerable.Range(0, 99)
                                      select (i, j);

            const int findValue = 19690720;

            return possibleAlterations.Select(alt =>
                {
                    originalInputs[1] = alt.i;
                    originalInputs[2] = alt.j;

                    return (alt, res: _c1.Run(originalInputs));
                })
                .Where(x => x.res == findValue)
                .Select(x => 100 * x.alt.i + x.alt.j)
                .Single();
        }
    }
}
