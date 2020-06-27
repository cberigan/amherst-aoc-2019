using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.C6
{
    public sealed class CelestialBody
    {
        public string Key { get; }
        private List<CelestialBody> _children;

        public IEnumerable<CelestialBody> ChildOrbits => 
            _children ?? Enumerable.Empty<CelestialBody>();

        public CelestialBody Parent { get; private set; }
        private IEnumerable<CelestialBody> _parents =>
            Parent == null 
            ? Enumerable.Empty<CelestialBody>() 
            : new[] { Parent }.Concat( Parent._parents);

        public IEnumerable<CelestialBody> AllParents => _parents;


        public CelestialBody(string key) => this.Key = key;


        internal void AddChild(CelestialBody value)
        {
            if (_children == null)
                _children = new List<CelestialBody>();
            _children.Add(value);
            if (value.Parent == null)
                value.Parent = this;
        }

        internal int StepsTo(CelestialBody body) =>
            AllParents.Select((p, i) => (p, i: i + 1))
                .Where(x => x.p == body)
                .Select(x => x.i)
                .First();

        public override string ToString() => Key.ToString();
        public override int GetHashCode() => Key.GetHashCode();
        public override bool Equals(object obj) => obj is CelestialBody c
                && c.Key == this.Key;

    }
}
