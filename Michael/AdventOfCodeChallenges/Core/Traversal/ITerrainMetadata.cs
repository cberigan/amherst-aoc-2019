namespace AdventOfCodeChallenges.Core.Traversal
{
    public interface ITerrainMetadata<T> { bool IsPassable(T terrain);
        bool IsPointOfInterest<T>(T tile);
    }
}
