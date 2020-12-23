using MoreLinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeChallenges.Core
{
    public class Tree<T> : IEnumerable<TreeNode<T>>
    {
        private readonly HashSet<TreeNode<T>> _items;

        public Tree()
        {
            _items = new HashSet<TreeNode<T>>(new TreeNodeEqualityComparer<T>());
        }
        public TreeNode<T> GetOrAdd(T obj)
        {
            var val = new TreeNode<T>(obj);
            if (!_items.TryGetValue(val, out TreeNode<T> node))
                _items.Add(val);
            return node ?? val;
        }

        public TreeNode<T> FindBy(Predicate<T> predicate) =>
            _items.FirstOrDefault(node => predicate(node.Value));

        public IEnumerator<TreeNode<T>> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

    }

    public sealed class TreeNode<T>
    {

        private List<TreeNode<T>> _linkedNodes;
        private TreeNode<T> _parent;

        public T Value { get; }

        public IEnumerable<TreeNode<T>> Parents =>
            _parent == null ? Enumerable.Empty<TreeNode<T>>()
            : new[] { _parent }.Concat(_parent.Parents);

        public IEnumerable<TreeNode<T>> ChildNodes => 
            _linkedNodes ?? Enumerable.Empty<TreeNode<T>>();

        public TreeNode(T obj)
        {
            this.Value = obj;
        }

        internal void AddChild(TreeNode<T> outerNode)
        {
            if (_linkedNodes == null)
                _linkedNodes = new List<TreeNode<T>>(1);
            _linkedNodes.Add(outerNode);
            if (outerNode._parent == null)
                outerNode._parent = this;
            this._parent?.AddChild(outerNode);
        }

        public TreeNode<T> AddChild(T element)
        {
            var n = new TreeNode<T>(element);
            AddChild(n);
            return n;
        }

        public override bool Equals(object obj)
        {
            return obj is TreeNode<T> node &&
                   EqualityComparer<T>.Default.Equals(Value, node.Value);
        }

        public override int GetHashCode() => HashCode.Combine(Value);
    }
}
