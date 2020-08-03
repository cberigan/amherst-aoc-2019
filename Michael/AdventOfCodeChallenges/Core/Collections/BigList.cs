using MoreLinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCodeChallenges.Core.Collections
{
    /// <summary>
    /// A list that allows writing to arbitrary positions. Also just barely enough to get a job done.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BigList<T> : IList<T>
    {
        private const int DefaultBucketSize = 10_000;

        private readonly List<List<T>> _buckets;
        private readonly int _bucketSize;

        public BigList() : this(DefaultBucketSize) { }

        public BigList(int bucketSize)
        {
            _buckets = new List<List<T>>() { new List<T>() };
            _bucketSize = bucketSize;
        }

        public BigList(IEnumerable<T> seed) : this() => AddRange(seed);

        public T this[int index] { get => this[(BigInteger)index]; set => this[(BigInteger)index] = value; }
        

        public T this[BigInteger index]
        {
            get
            {
                (List<T> bucket, int bucketIndex) bucketInfo = GetBucket(index);
                var item = GetItem(bucketInfo, index);
                return item;
            }
            set
            {
                var bucket = GetBucket(index);
                var itemIndex = GetItemIndex(bucket.bucketIndex, index);
                bucket.bucket[itemIndex] = value;
            }
        }



        public int Count => _buckets.Sum(bucket => bucket.Count);

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            var bucket = _buckets.FirstOrDefault(b => b.Count < _bucketSize);
            if (bucket == null)
            {
                bucket = new List<T>();
                _buckets.Add(bucket);
            }
            bucket.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            var bucket = _buckets.FirstOrDefault(b => b.Count < _bucketSize);
            if (bucket == null)
            {
                EnsureBucketCapacity(_buckets.Count + 1);
                bucket = _buckets.Last();
            }

            int added = 0;
            while (true)
            {
                
                var itemsToAdd = items.Take(_bucketSize - bucket.Count).Select(x => { added++; return x; });
                items = itemsToAdd;
                bucket.AddRange(itemsToAdd);

                if (bucket.Count < _bucketSize)
                    break;
                else
                {
                    bucket = new List<T>();
                    _buckets.Add(bucket);
                }
            }
        }

        public void Clear()
        {
            foreach (var l in _buckets)
                l.Clear();
        }

        public bool Contains(T item) => _buckets.Any(b => b.Contains(item));

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() => _buckets.SelectMany(b => b).GetEnumerator();

        public int IndexOf(T item) => _buckets.Select(b => b.IndexOf(item))
                .Where(i => i > -1)
                .DefaultIfEmpty(-1)
                .First();

        public void Insert(int index, T item)
        {
            var bucketInfo = GetBucket(index);
            var itemIndex = bucketInfo.bucket.IndexOf(item);
            if (itemIndex >= _bucketSize)
            {
                bucketInfo.bucket = new List<T>();
                _buckets.Add(bucketInfo.bucket);
                bucketInfo.bucket.Add(item);
            }
            else
            {
                EnsureItemCapacity(bucketInfo.bucket, itemIndex);
                bucketInfo.bucket.Insert(itemIndex, item);
            }
            
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => _buckets.SelectMany(b => b).GetEnumerator();


        private T GetItem((List<T> bucket, int bucketIndex) bucketInfo, BigInteger index)
        {
            var (bucket, bucketIndex) = bucketInfo;
            var itemIndex = GetItemIndex(bucketIndex, index);
            EnsureItemCapacity(bucket, itemIndex);
            var item = bucket[itemIndex];
            return item;
        }

        /// <summary>
        /// Returns an item's index within a particular bucket.
        /// </summary>
        private int GetItemIndex(int bucketIndex, BigInteger index)
        {
            var itemIndex = index - bucketIndex * _bucketSize;
            return (int)itemIndex;
        }

        private (List<T> bucket, int bucketIndex) GetBucket(BigInteger itemIndex)
        {
            var bucketIndex = BucketIndex(itemIndex);
            EnsureBucketCapacity(bucketIndex);

            var bucket = _buckets[bucketIndex];
            return (bucket, bucketIndex);
        }

        private int BucketIndex(BigInteger index) => (int)(index / _bucketSize);

        private void EnsureBucketCapacity(BigInteger bucketIndex)
        {
            var missingBuckets = bucketIndex + 1 - _buckets.Count;
            if (missingBuckets > 0)
            {
                while (missingBuckets > int.MaxValue)
                {
                    _buckets.AddRange(
                        Enumerable.Range(0, int.MaxValue).Select(_ => new List<T>()));
                    missingBuckets -= int.MaxValue;
                }

                _buckets.AddRange(
                    Enumerable.Range(0, (int)missingBuckets).Select(_ => new List<T>()));
            }
        }

        private void EnsureItemCapacity(List<T> bucket, int index)
        {
            var missingItems = index + 1 - bucket.Count;
            if (missingItems > 0)
                bucket.AddRange(Enumerable.Repeat(default(T), missingItems));
        }

    }
}
