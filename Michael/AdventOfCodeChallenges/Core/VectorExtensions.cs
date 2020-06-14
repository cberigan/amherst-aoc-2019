using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCodeChallenges.Core
{
    public static class VectorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Returns a Vector3 1, 0, or -1 for each axis.
        /// </summary>
        public static Vector3 CompareTo(this Vector3 a, Vector3 b) =>
            new Vector3(a.X.CompareTo(b.X),
                   a.Y.CompareTo(b.Y),
                   a.Z.CompareTo(b.Z));
    }
}
