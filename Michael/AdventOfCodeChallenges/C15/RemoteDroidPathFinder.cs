using AdventOfCodeChallenges.C10;
using System;

namespace AdventOfCodeChallenges.C15
{
    internal class RemoteDroidPathFinder
    {
        public event EventHandler<int> ShortestPathFound;

        public RemoteDroidPathFinder(RepairDroid repairDroid, RemoteControlProgram rcp)
        {
            var pathFinding = new PathFindingMap();
            pathFinding.Add(Coordinate.Origin);

            Coordinate oxygenTank = Coordinate.Origin;

            bool secondTimeAtOrigin = false;
            repairDroid.OnReply += (_, status) =>
            {
                // the idea is to stick to the right hand wall.
                // doing this does actually map out the whole map
                // if it runs until it gets back to the origin.
                // Once that is done, the path-finding path that's
                // been built up is guaranteed (in this test case)
                // to have built up the shortest map.
                if (rcp.CurrentPosition == Coordinate.Origin)
                {
                    if (secondTimeAtOrigin)
                    {
                        var shortestPath = pathFinding.ShortestPathSteps(oxygenTank);
                        Console.SetCursorPosition(0, 3);
                        Console.WriteLine("shortest path is {0}", shortestPath);
                        ShortestPathFound?.Invoke(this, shortestPath);
                    }
                    secondTimeAtOrigin = true;
                }
                else if (status == RepairDroidStatusCode.Moved || status == RepairDroidStatusCode.MovedAndFoundOxygenSystem)
                    pathFinding.Add(rcp.CurrentPosition);
            };
            rcp.FoundOxygenSystem += (_, coord) =>
            {
                oxygenTank = coord;
            };
        }
    }
}