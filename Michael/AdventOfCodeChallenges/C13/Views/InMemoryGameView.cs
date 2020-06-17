using AdventOfCodeChallenges.C10;
using System.Collections.Generic;

namespace AdventOfCodeChallenges.C13.Views
{
    public class InMemoryGameView : IGameView
    {
        private readonly Dictionary<Coordinate, TileType> _map;

        public IReadOnlyDictionary<Coordinate, TileType> Memory => _map;

        public InMemoryGameView() => _map = new Dictionary<Coordinate, TileType>();

        public void Draw(Coordinate coordinate, int e) => _map[coordinate] = (TileType)e;
    }
}
