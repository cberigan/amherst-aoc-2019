using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeChallenges.Core
{
    public sealed class TreeTextWriter
    {
        public string GetText<T>(Tree<T> tree)
        {
            var sb = new StringBuilder();
            int tabLevel = 0;
            foreach (var node in tree)
            {
                WriteTabLevel(sb, node, tabLevel);
            }
            return sb.ToString();
        }

        private void WriteTabLevel<T>(StringBuilder sb, TreeNode<T> node, int tabLevel)
        {
            sb.Append('\t', tabLevel).Append(node.Value).AppendLine();
            node.ChildNodes
                .Select((child, i) => (child, i))
                .Aggregate(0, (acc, cur) =>
                {
                    WriteTabLevel(sb, cur.child, acc + 1 + tabLevel);
                    return cur.i + 1;
                });
        }
    }
}
