using System.Collections;
using System.Collections.Generic;

namespace Nekki.Yaml
{
	public class Sequence : Node
	{
		public List<Node> nodesInside { get; private set; }

		public Sequence(string keyNew, Node[] nodes)
		{
			typeNode = "Sequence";
			key = keyNew;
			nodesInside = new List<Node>();
			if (nodes != null)
				nodesInside.AddRange(nodes);
		}

		public Sequence(string keyNew, List<Node> nodes) : this(keyNew, nodes?.ToArray()) { }
		public Sequence(string keyNew, Node node) : this(keyNew, new Node[] { node }) { }

		public int GetNodesSize() { return nodesInside.Count; }
		public Node GetNodesByIndex(int index)
		{
			return index >= 0 && index < nodesInside.Count ? nodesInside[index] : null;
		}
		public List<Node> GetNodesInside() { return nodesInside; }

		public void AddNode(Node newNode) { nodesInside.Add(newNode); }
		public void AddNodes(Node[] newNodes) { nodesInside.AddRange(newNodes); }
		public void AddNodes(List<Node> newNodes) { nodesInside.AddRange(newNodes); }
		public void Remove(Node newNode) { nodesInside.Remove(newNode); }
		public void RemoveNodes() { nodesInside.Clear(); }

		public override IEnumerator GetEnumerator()
		{
			foreach (Node item in nodesInside)
				yield return item;
		}
	}
}
