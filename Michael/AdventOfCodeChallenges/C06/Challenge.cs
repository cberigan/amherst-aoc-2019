using AdventOfCodeChallenges.Core;
using System;
using System.Text;

namespace AdventOfCodeChallenges.C6
{
    public sealed partial class Challenge
    {
        private readonly OrbitParser _parser;

        public Challenge() => _parser = new OrbitParser();
        public int Run()
        {
            var parsedResult = _parser.Parse(Input);
            return new OrbitalSystem(parsedResult).DirectAndIndirectOrbitCount;
        }

        public sealed class Pt2
        {
            private readonly OrbitParser _parser;

            public Pt2() => _parser = new OrbitParser();
            public int Run(string input = null)
            {
                var parsedResult = _parser.Parse(input ?? Input);
                var system = new OrbitalSystem(parsedResult);
                var navigationStepCount = system.Navigate("YOU", "SAN");
                return navigationStepCount;
            }
        }
    }
}
