using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeChallenges.C16
{
    public struct RepeatingDigits
    {
        private readonly int[] _source;
        private readonly int _repeats;

        public int Length { get; }


        public RepeatingDigits(int[] source, int timesToRepeat) => 
            (_source, _repeats, Length) = (source, timesToRepeat, source.Length * timesToRepeat);


        public int this[int i]
        {
            get
            {
                if (i > Length) throw new ArgumentOutOfRangeException("index");
                var x = _source[i / _repeats % _source.Length];
                return x;
            }
        }


        public Enumerator GetEnumerator() => new Enumerator(this);


        public struct Enumerator : IEnumerator<int>
        {
            private RepeatingDigits _source;
            private int _index, _repeated;

            public Enumerator(RepeatingDigits repeatingDigits) => 
                (_source, _index, _repeated) = (repeatingDigits, 0,1);

            public int Current => _source._source[_index];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_repeated < _source._repeats)
                    _repeated++;
                else
                {
                    _repeated = 1;
                    _index++;
                }

                if (_index * _source._repeats + _repeated -1 < _source.Length)
                    return true;
                return false;
            }

            public void Reset()
            {
            }
        }
    }
}
