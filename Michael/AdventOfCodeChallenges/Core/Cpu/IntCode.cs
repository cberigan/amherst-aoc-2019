using AdventOfCodeChallenges.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCodeChallenges.Core.Cpu.OpCode;

namespace AdventOfCodeChallenges.Core.Cpu
{
    public class IntCodeStateMachine
    {
        private int nextInput;
        private bool _halted;

        public Instruction Instruction { get; private set; }
        public bool IsHalted => Instruction.Op == OpCode.Halt || _halted;
        public int? Phase { get; set; }
        public int NextWriteInstruction { get; set; }
        public BigList<int> Memory { get; set; }
        public int CurrentOffset { get; private set; }
        public int CurrentOutputValue { get; private set; }
        public bool IsAtEnd => CurrentOffset >= Memory.Count;
        public int RelativeBaseOffset = 0;

        public event EventHandler<int> OnOutput;

        public IntCodeStateMachine(IEnumerable<int> memory, int? phase = null)
        {
            Memory = new BigList<int>(memory);
            Phase = phase;
        }

        public void SetInput(int i) => nextInput = i;

        public void Reset()
        {
            CurrentOffset = 0;
            _halted = true;
        }

        public void RunOnce()
        {
            if (IsHalted) return;
            Instruction = Read();
            Arguments args = Decode();
            Execute(args);
        }

        public void RunAll()
        {
            while (!IsHalted)
                RunOnce();
        }

        public void Halt() 
        {
            CurrentOffset = Memory.Count;
            _halted = true;
        }

        private Instruction Read()
        {
            if (Memory[CurrentOffset] == 0)
                System.Diagnostics.Debugger.Break();
            return new Instruction(Memory[CurrentOffset]);
        }

        private Arguments Decode()
        {
            var len = OpCodeArgLength(Instruction.Op);
            return new Arguments(Memory, Instruction, len, CurrentOffset + 1, RelativeBaseOffset);
        }

        private void Execute(Arguments args)
        {
            var memory = Memory;
            var ins = Instruction;

            switch (Instruction.Op)
            {
                case Add:
                    memory[args.TargetIndex] = args.Arg1 + args.Arg2;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case Mul:
                    memory[args.TargetIndex] = args.Arg1 * args.Arg2;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case Write:
                    memory[args.TargetIndex] = Phase.GetValueOrDefault(nextInput);
                    Phase = null;
                    CurrentOffset += 2;
                    break;
                case OpCode.Read:
                    CurrentOutputValue = args.Arg1;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    OnOutput?.Invoke(this, CurrentOutputValue);
                    break;
                case JumpIfTrue:
                    if (args.Arg1 != 0)
                        CurrentOffset = args.Arg2;
                    else
                        CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case JumpIfFalse:
                    if (args.Arg1 == 0)
                        CurrentOffset = args.Arg2;
                    else CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case LT:
                    memory[args.TargetIndex] = args.Arg1 < args.Arg2 ? 1 : 0;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case E:
                    memory[args.TargetIndex] = args.Arg1 == args.Arg2 ? 1 : 0;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    break;
                case AdjustRelativeBase:
                    RelativeBaseOffset += args.Arg1;
                    CurrentOffset += OpCodeArgLength(ins.Op) + 1;
                    //if (RelativeBaseOffset > memory.Count)
                    //    memory.AddRange(Enumerable.Repeat(0, RelativeBaseOffset - memory.Count));
                    break;
                case OpCode.Halt:
                    break;
            }
        }


        static int OpCodeArgLength(OpCode op)
        {
            return op switch
            {
                Add => 3,
                Mul => 3,
                Write => 1,
                OpCode.Read => 1,
                OpCode.Halt => 1,
                JumpIfTrue => 2,
                JumpIfFalse => 2,
                LT => 3,
                E => 3,
                AdjustRelativeBase => 1,
                _ => 1 // halt. since I'm no longer jumping to the end of the memory, this might be a bug now
            };
        }

        //private class GrowingList<T> where T : struct
        //{
        //    private T[] _items;
        //    private int _currentLength = 0;
        //    public GrowingList(IEnumerable<T> source)
        //    {
        //        if (source is IList<T> l)
        //        {
        //            _items = new T[l.Count];
        //            l.CopyTo(_items, 0);
        //        }
        //        else
        //            AddRange(source);
        //    }

        //    public ref T this[int i]
        //    {
        //        get { return ref _items[i]; }
                
        //    }

        //    public T this[int i]
        //    {
        //        set { _items[i] = value; }
        //    }
                

        //    public void Add(T item)
        //    {
        //        EnsureCapacity(_currentLength + 1);
        //        _currentLength++;
        //        _items[_currentLength + 1] = item;
        //    }

        //    public void AddRange(IEnumerable<T> source)
        //    {
        //        if (source is IList<T> l)
        //        {
        //            var lastLength = _items.Length;
        //            EnsureCapacity(_currentLength + l.Count);
        //            l.CopyTo(_items, lastLength);
        //            _currentLength += l.Count;
        //        }
        //        else
        //        {
        //            foreach (var item in source)
        //                Add(item);
        //        }
        //    }

        //    private void EnsureCapacity(int size)
        //    {
        //        var finalLength = _currentLength + size;
        //        if (_items.Length <= finalLength)
        //        {
        //            var increases = finalLength / _items.Length;
        //            var newArray = new T[_items.Length * increases];
        //            Array.Copy(_items, newArray, _items.Length);
        //            _items = newArray;
        //        }
        //    }
        //}
    }
}
