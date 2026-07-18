using System;
using System.Collections.Generic;
using Godot;
using SF3;
using SF3.Items;
using SF3.GameModels;

public partial class ShadowPerksSlotsHolder : Node
{
	[Export] public int maxSlots = 4;

	private List<Node> _slots = new List<Node>();
	private Model _model;

	public bool Inverted { get; set; }

	public bool Visible
	{
		get => Visible;
		set { foreach (Node slot in _slots) if (slot is CanvasItem ci) ci.Visible = value; }
	}

	public void Init()
	{
		_slots.Clear();
		foreach (Node child in GetChildren())
			_slots.Add(child);
	}

	public void SetModel(Model model)
	{
		_model = model;
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
		return (index >= 0 && index < _slots.Count) ? _slots[index] : null;
	}

	public void SetShadowPerkState(ShadowPerksState state)
	{
		// Set alpha/visibility for all slots based on state
		float alpha = SF3.ShadowPerksController.GetAlpha(state);
		foreach (Node slot in _slots)
			if (slot is CanvasItem ci) ci.Modulate = new Color(ci.Modulate.R, ci.Modulate.G, ci.Modulate.B, alpha);
	}

	public void ClearShadowPerkSlot(EquipmentType equipmentType)
	{
		GD.Print($"ShadowPerksSlotsHolder.ClearShadowPerkSlot: {equipmentType}");
	}

	public void StartCooldown(EquipmentType equipmentType, int framesDuration)
	{
		GD.Print($"ShadowPerksSlotsHolder.StartCooldown: {equipmentType} for {framesDuration} frames");
	}

	public void Clear()
	{
		foreach (Node slot in _slots)
			slot.QueueFree();
		_slots.Clear();
	}
}
