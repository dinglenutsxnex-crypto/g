using System;
using System.Collections.Generic;
using Godot;

public class ShadowPerksSlotsHolder : Node
{
	[Export]
	public int maxSlots = 4;

	private List<Node> _slots = new List<Node>();

	public override void _Ready()
	{
		GD.Print("STUB: ShadowPerksSlotsHolder._Ready");
	}

	public void AddSlot(Node slot)
	{
		if (_slots.Count < maxSlots)
		{
			_slots.Add(slot);
			AddChild(slot);
		}
	}

	public void RemoveSlot(int index)
	{
		if (index >= 0 && index < _slots.Count)
		{
			Node slot = _slots[index];
			_slots.RemoveAt(index);
			slot.QueueFree();
		}
	}

	public Node GetSlot(int index)
	{
		if (index >= 0 && index < _slots.Count)
			return _slots[index];
		return null;
	}

	public void Clear()
	{
		foreach (Node slot in _slots)
		{
			slot.QueueFree();
		}
		_slots.Clear();
	}
}
