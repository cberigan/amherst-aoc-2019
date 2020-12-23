using AdventOfCodeChallenges.Core.Traversal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.C18
{
    public class SimpleSequentialPathFinder
    {
        private readonly SortedList<char, int> _sortedList = new System.Collections.Generic.SortedList<char, int>();

        public int ShortestSteps => _sortedList.Sum(kvp => kvp.Value);

        public void Add(char c, int steps)
        {
            var added = _sortedList.TryAdd(c, steps);
        }

        public bool Contains(char c) => _sortedList.ContainsKey(c);

        public void Monitor(ITraversalStrategy<char> traversalStrategy) => 
            traversalStrategy.OnFound(char.IsLower, (c, pos, steps) => Add(c, steps));

        public char LastKey() => _sortedList.Keys.Last();
    }
}
