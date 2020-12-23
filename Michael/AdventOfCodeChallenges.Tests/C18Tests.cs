using AdventOfCodeChallenges.C18;
using AdventOfCodeChallenges.Core.Traversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCodeChallenges.Tests
{
    public class C18Tests
    {
        [Theory]
        //[InlineData(Tunnel1, 8, 'b')]
        [InlineData(Tunnel2, 86, 'f')]
        public void Test1(string tunnelLayout, int expectedSteps, char stopChar)
        {
            var tunnel = Tunnel.Parse(tunnelLayout);

            var shortestPathFinder = new SimpleSequentialPathFinder();

            var terrainMetadata = new TunnelTerrainMetadata();
            var traversalStrategy = new StickRightTraversalStrategy(terrainMetadata);

            terrainMetadata.Monitor(traversalStrategy);
            shortestPathFinder.Monitor(traversalStrategy);

            bool stop = false;
            traversalStrategy.OnFound(c => c == stopChar, (_,__,___) => stop = true);
            traversalStrategy.Traverse(tunnel, tunnel.Entrance, () => stop, resetStepCounter: c => char.IsLower(c) && shortestPathFinder.LastKey() == c);

            var shortestSteps = shortestPathFinder.ShortestSteps;

            Assert.Equal(expectedSteps, shortestSteps);
        }

        public void MoreAdvancedTest()
        {
            char[] shortestPath = { 'b', 'a', 'c', 'd', 'f', 'e', 'g' };
            const int shortestSteps = 132;
            const char stopChar = 'g';

            var tunnel = Tunnel.Parse(Tunnel3);

            var shortestPathFinder = new LinkedNodeShortestPathFinder();

            var terrainMetadata = new TunnelTerrainMetadata();
            var traversalStrategy = new StickRightTraversalStrategy(terrainMetadata);

            terrainMetadata.Monitor(traversalStrategy);
            shortestPathFinder.Monitor(traversalStrategy);
            shortestPathFinder.Monitor(terrainMetadata);
            

            bool stop = false;
            traversalStrategy.OnFound(c => c == stopChar, (_, __, ___) => stop = true);
            traversalStrategy.Traverse(tunnel, tunnel.Entrance, () => stop, resetStepCounter: c => char.IsLower(c) && shortestPathFinder.LastKey() == c);

            var shortestSteps = shortestPathFinder.ShortestSteps;

            Assert.Equal(expectedSteps, shortestSteps);
        }


        private const string Tunnel1 = @"#########
#b.A.@.a#
#########";

        private const string Tunnel2 = @"########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################";

        private const string Tunnel3 = @"########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################";
    }
}
