using AdventOfCodeChallenges.Core;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCodeChallenges.C12
{
    public static class PlanetarySystem
    {
        public static (Vector3 position, Vector3 velocity)[] Step(
            (Vector3 position, Vector3 velocity)[] system
            )
        {
            var output = new (Vector3 position, Vector3 velocity)[system.Length];
            Step(system, output);
            return output;
        }

        /// <summary>
        /// Overload of the Step function that makes 0 allocations.
        /// </summary>
        /// <param name="output">A user provided and initialized output array. Can be blank, but must be the same length as system</param>
        /// <param name="system">Current state.</param>
        public static void Step(
            (Vector3 position, Vector3 velocity)[] system,
            (Vector3 position, Vector3 velocity)[] output
        )
        {
            Array.Copy(system, output, system.Length);
            VelocityDiff(system, output);

            for (int i = 0; i < system.Length; i++)
            {
                var newVelocity = output[i].velocity;
                output[i].position = system[i].position + newVelocity;
            }
        }

        private static void VelocityDiff((Vector3 position, Vector3 velocity)[] system, (Vector3 position, Vector3 velocity)[] output)
        {
            for (int i = 0; i < system.Length; i++)
                for (int j = i + 1; j < system.Length; j++)
                {
                    output[i].velocity += system[j].position.CompareTo(system[i].position);
                    output[j].velocity += system[i].position.CompareTo(system[j].position);
                }
        }


        public static float TotalEnergy(IEnumerable<(Vector3 position, Vector3 velocity)> system) =>
            system.Aggregate(0f, (acc, cur) =>
            {
                var potentialEnergy = Math.Abs(cur.position.X) + Math.Abs(cur.position.Y) + Math.Abs(cur.position.Z);
                var kineticEnergy = Math.Abs(cur.velocity.X) + Math.Abs(cur.velocity.Y) + Math.Abs(cur.velocity.Z);
                return potentialEnergy * kineticEnergy + acc;
            });
    }
}
