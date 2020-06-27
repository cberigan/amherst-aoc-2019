using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2019.Day6
{
	class Program
	{
		static void Main(string[] args)
		{

			var lines = File.ReadAllLines("input.txt");
			//part1
			//var lines = new string[]  {"COM)B","B)C","C)D","D)E","E)F","B)G","G)H","D)I","E)J","J)K","K)L" };
			//part2
			//var lines = new string[] { "COM)B","B)C","C)D","D)E","E)F","B)G","G)H","D)I","E)J","J)K","K)L","K)YOU","I)SAN"};
			var counter = new TreeCounter();


			Console.WriteLine("Answer: " + counter.CountOrbits(lines));
			Console.ReadLine();


		}


		public class TreeCounter
		{
			private Dictionary<string, Node> nodeMap;
			public (int, int) CountOrbits(string[] lines)
			{
				nodeMap = new Dictionary<string, Node>();
				Node comNode = null;
				Node youNode = null;
				Node santaNode = null;
				// build tree
				foreach (var item in lines)
				{
					var elems = item.Split(')');
					var parentId = elems[0];
					var childId = elems[1];

					var parentNode = GetNode(parentId);
					var childNode = GetNode(childId);
					if (parentId == "COM") comNode = parentNode;
					if (childId == "YOU") youNode = parentNode;
					if (childId == "SAN") santaNode = parentNode;

					parentNode.Children.Add(childNode);
					childNode.Parent = parentNode;
				}

				//search tree
				return (DFS(comNode, 0), OrbitalSearch(youNode, santaNode));
			}

			private int DFS(Node currentNode, int level)
			{
				int masterSum = 0;
				level++;
				foreach (var child in currentNode.Children)
				{
					masterSum += level + DFS(child, level);
				}

				return masterSum;
			}

			public int OrbitalSearch(Node youNode, Node santaNode)
			{
				Dictionary<string, int> youPathCount = new Dictionary<string, int>();

				Node currentNode = youNode;
				int count = 0;
				while (currentNode.Parent != null)
				{
					currentNode = currentNode.Parent;
					count++;
					youPathCount.Add(currentNode.Id, count);
				}

				currentNode = santaNode;
				count = 0;
				while (currentNode.Parent != null)
				{
					currentNode = currentNode.Parent;
					count++;
					if (youPathCount.ContainsKey(currentNode.Id))
					{
						return count + youPathCount[currentNode.Id];
					}

				}
				return 0;
			}

			private Node GetNode(string id)
			{
				if (nodeMap.ContainsKey(id))
				{
					return nodeMap[id];
				}
				else
				{
					var newNode = new Node(id);
					nodeMap.Add(id, newNode);
					return newNode;
				}
			}
		}


		
	}


	public class Node
	{
		public string Id { get; set; }
		//public Node Left { get; set; }
		//public Node Right { get; set; }
		public Node Parent { get; set; }
		public List<Node> Children { get; set; } = new List<Node>();
		public Node(string id)
		{
			Id = id;
		}


	}
}
