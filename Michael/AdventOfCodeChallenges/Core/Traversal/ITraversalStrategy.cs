using AdventOfCodeChallenges.C10;
using System;

namespace AdventOfCodeChallenges.Core.Traversal
{

    public interface ITraversalStrategy<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="callback">tile type, coordinate, number of steps</param>
        /// <param name=""></param>
        void OnFound(Predicate<T> predicate, Action<T, Coordinate, int> callback);

        void Traverse(ITraversable<char> traversable, Coordinate startingPoint, Func<bool> stop, Predicate<char> resetStepCounter);
    }
}