using AdventOfCodeChallenges.Core;
using System;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.C18
{
    public sealed class TunnelTerrainParser : IParser2<TunnelTerrain>
    {
        public IEnumerable<TunnelTerrain> Parse(string input)
        {
            foreach (var floorTile in input)
            {
                if (char.IsWhiteSpace(floorTile)) continue;

                yield return floorTile switch
                {
                    '#' => TunnelTerrain.Wall,
                    '.' => TunnelTerrain.Floor,
                    '@' => TunnelTerrain.Entrance,
                    _ when char.IsLower(floorTile) => TunnelTerrain.Key,
                    _ when char.IsUpper(floorTile) => TunnelTerrain.Door,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}
