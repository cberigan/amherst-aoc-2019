using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeChallenges.C16
{
    public struct RepeatingDigits
    {
        private readonly sbyte[] _source;
        private readonly int _repeats;

        public RepeatingDigits(sbyte[] source, int timesToRepeat) => 
            (_source, _repeats) = (source, timesToRepeat);


        public sbyte this[int i]
        {
            get
            {
                var x = _source[i / _repeats % _source.Length];
                return x;
            }
        }


        //public Enumerator GetEnumerator() => new Enumerator(this);


        //public struct Enumerator : IEnumerator<int>
        //{
        //    private RepeatingDigits _source;
        //    private int _index, _repeated, _current;

        //    public Enumerator(RepeatingDigits repeatingDigits) => 
        //        (_source, _index, _repeated, _current) = (repeatingDigits, 0,1,0);

        //    public int Current => _current;

        //    object IEnumerator.Current => Current;

        //    public void Dispose()
        //    {
        //    }

        //    public bool MoveNext()
        //    {
        //        if (_repeated < _source._repeats)
        //            _repeated++;
        //        else
        //        {
        //            _repeated = 1;
        //            _index++;
        //        }

        //        if (_index * _source._repeats + _repeated - 1 < _source.Length)
        //        {
        //            _current = _source._source[_index];
        //            return true;
        //        }
        //        return false;
        //    }

        //    public void Reset()
        //    {
        //    }
        //}
    }
}
