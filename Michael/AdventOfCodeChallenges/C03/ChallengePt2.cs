using AdventOfCodeChallenges.Core;
using MoreLinq;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCodeChallenges.C3
{
    public sealed class ChallengePt2
    {
        public double Run(string path1 = null, string path2 = null)
        {
            var wirePath1 = path1 == null ? ChallengePt1.Wire1Paths : ChallengePt1.ParsePaths(path1);
            var wirePath2 = path2 == null ? ChallengePt1.Wire2Paths : ChallengePt1.ParsePaths(path2);

            var (wireLines1, wireLines2) = ChallengePt1.GetLines(wirePath1, wirePath2);

            var withSteps1 = GetLinesWithSteps(wireLines1);
            var withSteps2 = GetLinesWithSteps(wireLines2);

            var query = from a in withSteps1
                        from b in withSteps2
                        let intersect = a.line.Intersects(b.line)
                        where intersect.intersects && intersect.point != new Point()
                        let aclipped = a.line.ClipAt(intersect.point)
                        let bclipped = b.line.ClipAt(intersect.point)
                        let intersects = (intersect.point, steps:a.steps - aclipped.distance + b.steps - bclipped.distance)  
                        group intersects by intersect.point into ints
                        select (ints.Key, steps: ints.Min(i => i.steps));

            var best = query.Min(x => x.steps);
            return best;
        }

        private List<(Line line, int steps)> GetLinesWithSteps(IEnumerable<Line> l) =>
            l.Aggregate(new List<(Line lines, int steps)>(ChallengePt1.Wire1Paths.Count) { (new Line(new Point(), new Point()), 0) },
                (acc, curr) =>
                {
                    acc.Add((curr, acc.Last().steps + curr.Steps()));
                    return acc;
                });
    }
}
