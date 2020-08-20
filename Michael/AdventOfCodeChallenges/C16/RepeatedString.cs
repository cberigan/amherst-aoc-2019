using System;

namespace AdventOfCodeChallenges.C16
{
    public readonly struct RepeatedString
    {
        public int Repeats { get; }

        public string InternalString { get; }

        public int Length
        {
            get
            {
                checked
                {
                    return InternalString.Length * Repeats;
                }
            }
        }

        public RepeatedString(string s, int repeats) => (InternalString, Repeats) = (s, repeats);

        public char this[int i]
        {
            get
            {
                if (i > Length) throw new ArgumentOutOfRangeException("index");
                return InternalString[i % InternalString.Length];
            }
        }

    }
}
