using System;
using Godot;

namespace SF3.Items
{
	public partial class PerkSlot : Node
	{
		[Export]
		public int slotIndex;

		[Export]
		public TextureRect slotIcon;

		[Export]
		public Label slotLabel;

		public Perk equippedPerk;
		public bool Empty { get { return equippedPerk == null; } }
		public int id { get { return slotIndex; } }
		public int index { get { return slotIndex; } }
		public Perk perk { get { return equippedPerk; } set { equippedPerk = value; } }

		public override void _Ready()
		{
			GD.Print("STUB: PerkSlot._Ready");
		}

		public bool IsPerkType(object perk)
		{
			return false;
		}

		public void SetItem(object item, bool animate, EffectType effectType)
		{
			GD.Print("STUB: PerkSlot.SetItem");
		}

		public void ChangeItem(object slot, object remove, object add)
		{
			GD.Print("STUB: PerkSlot.ChangeItem");
		}

		public void RemoveItem()
		{
			equippedPerk = null;
		}
	}
}
