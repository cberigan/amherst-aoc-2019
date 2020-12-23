using AdventOfCodeChallenges.C10;
using AdventOfCodeChallenges.Core;
using AdventOfCodeChallenges.Core.Traversal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCodeChallenges.C18.Tunnel;

namespace AdventOfCodeChallenges.C18
{
    public partial class Challenge
    {
        
        public Challenge()
        {
            
        }


        public int Run(string input)
        {
            Tunnel tunnel = Tunnel.Parse(input);
            var position = tunnel.Entrance;

            var terrainMetadata = new TunnelTerrainMetadata();

            var traversingStrategy = new StickRightTraversalStrategy(
                terrainMetadata: terrainMetadata);
            //traversingStrategy.OnFound(char.IsLower, c => terrainMetadata.Keys.Add(c));

            //traversingStrategy.Traverse((ITraversable)tunnel, startingPoint:position,)

            return 0;
        }
    }

    public class Tunnel : ITraversable<char>
    {
        private readonly string map;
        private Coordinate _entrance = Coordinate.Origin;
        private int _width = -1;

        public int Width
        {
            get
            {
                if (_width == -1)
                {
                    _width = map.IndexOf("\r\n");
                }
                return _width;
            }
        }

        public Coordinate Entrance
        { 
            get
            {
                if (_entrance == Coordinate.Origin)
                {
                    var idx = map.IndexOf('@');
                    var y = idx / Width;
                    var newLineCharacters = y * 2;
                    var x = idx % Width - newLineCharacters;
                    _entrance = (x, y);
                }

                return _entrance;
            } 
        }

        public Coordinate OriginCoordinates => Entrance;

        public char OriginTile => '@';

        public char this[Coordinate c]
        {
            get
            {
                var lineOffset = c.Y * Width;
                var missingNewLineCharacters = c.Y * 2;
                var pos = c.X + lineOffset + missingNewLineCharacters;
                return map[pos];
            }
        }

        protected Tunnel(string map)
        {
            this.map = map;
        }
        public static Tunnel Parse(string map) => new Tunnel(map);

    }

    public class TunnelTerrainMetadata : ITerrainMetadata<char>
    {
        public HashSet<char> Keys { get; } = new HashSet<char>();
        public event EventHandler<char> TriedOpenLockedDoor;

        public bool IsPassable(char terrainSquare) => terrainSquare switch
        {
            '#' => false,
            '.' => true,
            '@' => true,
            _ when char.IsLower(terrainSquare) => true,
            _ when char.IsUpper(terrainSquare) && Keys.Contains(char.ToLower(terrainSquare)) => true,
            _ => NotifyLockedDoor(terrainSquare) // false
        };

        bool NotifyLockedDoor(char terrainSquare)
        {
            TriedOpenLockedDoor?.Invoke(this, terrainSquare);
            return false;
        }

        public void Monitor(ITraversalStrategy<char> traversalStrategy)
        {
            traversalStrategy.OnFound(char.IsLower, (c, _, __) => Keys.Add(c));
        }

        public bool IsPointOfInterest<T>(T tile)
        {
            throw new NotImplementedException();
        }
    }
}
