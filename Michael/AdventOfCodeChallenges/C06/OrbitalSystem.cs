using AdventOfCodeChallenges.Core;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.C6
{
    public sealed class OrbitalSystem
    {
        private readonly Tree<CelestialBody> _orbits;

        public CelestialBody Origin { get; }

        public int DirectAndIndirectOrbitCount => _orbits.SelectMany(node => node.Parents).Count();

        public OrbitalSystem(IEnumerable<(CelestialBody inner, CelestialBody outer)> orbits)
        {
            var tree = new Tree<CelestialBody>();
            foreach (var (inner, outer) in orbits)
            {
                var innerNode = tree.GetOrAdd(inner);
                var outerNode = tree.GetOrAdd(outer);
                innerNode.AddChild(outerNode);

                innerNode.Value.AddChild(outerNode.Value);

                if (Origin == null)
                {
                    var comHash = "COM".GetHashCode();
                    if (inner.GetHashCode() == comHash)
                        Origin = inner;
                    else if (outer.GetHashCode() == comHash)
                        Origin = outer;
                }
            }

            _orbits = tree;
        }

        public int Navigate(string origin, string destination)
        {
            var originNode = _orbits.FindBy(body => body.Key == origin).Parents.First();
            var destinationNode = _orbits.FindBy(body => body.Key == destination).Parents.First();

            var traversalSteps = TraversalSteps(originNode.Value, destinationNode.Value);
            return traversalSteps;
        }


        public int TraversalSteps(CelestialBody origin, CelestialBody destination)
        {
            var distance = (
                from op in origin.AllParents
                join dp in destination.AllParents
                on op.Key equals dp.Key
                select (op, steps: origin.StepsTo(op) + destination.StepsTo(dp)))
                .MinBy(x => x.steps)
                .First()
                .steps;

            return distance;
        }

        public override string ToString() =>
            new TreeTextWriter().GetText(_orbits); // eh, doesn't really work right

    }
}
