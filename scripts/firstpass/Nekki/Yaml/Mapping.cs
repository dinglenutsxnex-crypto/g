using System;
using System.Collections;
using System.Collections.Generic;

namespace Nekki.Yaml
{
	[Serializable]
	public class Mapping : Node, ICollection, IEnumerator, IEnumerable
	{
		private int indexPosition = -1;
		public List<Node> nodesInside { get; private set; }

		public int Count { get { return nodesInside?.Count ?? 0; } }
		public bool IsSynchronized { get { return false; } }
		public object SyncRoot { get { return null; } }
		public object Current { get { return nodesInside[indexPosition]; } }

		public Mapping(string keyNew)
		{
			typeNode = "Mapping";
			key = keyNew;
			nodesInside = new List<Node>();
		}

		public Mapping(string keyNew, Node[] entries) : this(keyNew)
		{
			if (entries != null)
				nodesInside.AddRange(entries);
		}

		public Mapping(string keyNew, Node entry) : this(keyNew, new Node[] { entry }) { }
		public Mapping(string keyNew, List<Node> entries) : this(keyNew, entries?.ToArray()) { }

		public int GetNodesSize() { return nodesInside.Count; }
		public Node GetNodesByIndex(int index)
		{
			return index >= 0 && index < nodesInside.Count ? nodesInside[index] : null;
		}
		public List<Node> GetNodesInside() { return nodesInside; }

		public void Add(Node entry)
		{
			if (entry != null)
				nodesInside.Add(entry);
		}

		public void AddNodes(List<Node> newNodes) { AddNodes(newNodes?.ToArray()); }
		public void AddNodes(Node[] newNodes)
		{
			if (newNodes != null)
				foreach (Node entry in newNodes) Add(entry);
		}

		public void Remove(string keyName, string valueName)
		{
			nodesInside.RemoveAll(n => n.key == keyName && n.value?.ToString() == valueName);
		}

		public void Remove(Node nodeToDelete)
		{
			if (nodeToDelete != null)
				nodesInside.Remove(nodeToDelete);
		}

		public Mapping GetMapping(string name)
		{
			foreach (Node item in nodesInside)
				if (item.key == name && item is Mapping)
					return (Mapping)item;
			return null;
		}

		public Mapping GetMappingByPath(string path)
		{
			string[] parts = path.Split(':');
			Mapping current = this;
			foreach (string name in parts)
			{
				current = current?.GetMapping(name);
				if (current == null) return null;
			}
			return current;
		}

		public Sequence GetSequence(string name)
		{
			foreach (Node item in nodesInside)
				if (item.key == name && item is Sequence)
					return (Sequence)item;
			return null;
		}

		public Scalar GetText(string name)
		{
			foreach (Node item in nodesInside)
				if (item.key == name && item is Scalar)
					return (Scalar)item;
			return null;
		}

		public void SetOrAddText(string nodeName, string text)
		{
			Scalar existing = GetText(nodeName);
			if (existing != null)
				existing.SetText(text);
			else
				Add(new Scalar(nodeName, text));
		}

		public Node GetNode(string name)
		{
			foreach (Node item in nodesInside)
				if (item.key == name)
					return item;
			return null;
		}

		public override IEnumerator GetEnumerator()
		{
			foreach (Node item in nodesInside)
				yield return item;
		}

		public void CopyTo(Array array, int index)
		{
			for (int i = index; i < array.Length && i - index < nodesInside.Count; i++)
				array.SetValue(nodesInside[i - index], i);
		}

		public bool MoveNext()
		{
			if (indexPosition == nodesInside.Count - 1) { Reset(); return false; }
			indexPosition++;
			return true;
		}

		public void Reset() { indexPosition = -1; }
	}
}
