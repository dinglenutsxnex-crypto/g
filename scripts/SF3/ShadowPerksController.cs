using System;
using System.Collections.Generic;
using Godot;
using SF3.Items;

public class ShadowPerksController : Node
{
	[Export]
	private ShadowPerksSlotsHolder _slotsHolder;

	private List<SF3.Items.Perk> _equippedPerks = new List<SF3.Items.Perk>();

	public void EquipPerk(SF3.Items.Perk perk)
	{
		if (!_equippedPerks.Contains(perk))
		{
			_equippedPerks.Add(perk);
			GD.Print("ShadowPerksController.EquipPerk: " + perk.id);
		}
	}

	public void UnequipPerk(SF3.Items.Perk perk)
	{
		_equippedPerks.Remove(perk);
		GD.Print("ShadowPerksController.UnequipPerk: " + perk.id);
	}

	public bool HasPerkEquipped(SF3.Items.Perk perk)
	{
		return _equippedPerks.Contains(perk);
	}

	public List<SF3.Items.Perk> GetEquippedPerks()
	{
		return new List<SF3.Items.Perk>(_equippedPerks);
	}

	public void Clear()
	{
		_equippedPerks.Clear();
	}
}
