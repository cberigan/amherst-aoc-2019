using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCodeChallenges.Core
{
    internal class TreeNodeEqualityComparer<T> : IEqualityComparer<TreeNode<T>>
    {
        public bool Equals([DisallowNull] TreeNode<T> x, [DisallowNull] TreeNode<T> y) =>
            Equals(x.Value, y.Value);

        public int GetHashCode([DisallowNull] TreeNode<T> obj) => obj.Value.GetHashCode();
    }
}