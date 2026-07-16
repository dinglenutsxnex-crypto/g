using System;
using System.Collections.Generic;
using Godot;

public class SplitController : Node
{
	private List<Node> _splits = new List<Node>();

	[Export]
	private Node _splitContainer;

	[Export]
	private Node _splitPrefab;

	public void AddSplit(Node split)
	{
		_splits.Add(split);
		if (_splitContainer != null)
		{
			_splitContainer.AddChild(split);
		}
	}

	public void RemoveSplit(Node split)
	{
		_splits.Remove(split);
		if (split != null)
		{
			split.QueueFree();
		}
	}

	public void Clear()
	{
		foreach (Node split in _splits)
		{
			if (split != null)
				split.QueueFree();
		}
		_splits.Clear();
	}

	public int Count
	{
		get { return _splits.Count; }
	}

	public Node GetSplit(int index)
	{
		if (index >= 0 && index < _splits.Count)
			return _splits[index];
		return null;
	}

	public void Refresh()
	{
		GD.Print("SplitController.Refresh");
	}
}
