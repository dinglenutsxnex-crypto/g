// SKELETON STUB: SF3/PerkInfoDrawer.cs
// [MONO] Original used NGUI (UITable, NekkiUILabel, UISprite, UITexture).
// Needs full UI rebuild with Godot Control nodes.
using System.Collections.Generic;
using Godot;
using Nekki.UI;
using SF3.Items;

namespace SF3
{
	public partial class PerkInfoDrawer : Node
	{
		public Node perkTable;

		public NekkiUILabel bonusLabel;

		public Node bonusSprite;

		public Node perkInfoPrf;

		public Node perkInfoEmptyPrf;

		private Node mainTable;

		public override void _Ready()
		{
			mainTable = this;
		}

		public void Draw(BaseItem item)
		{
			Clear();
			ISlotable slotable = item as ISlotable;
			if (slotable != null)
			{
				List<ItemSlot> slotItems = slotable.GetSlotItems();
				DrawPerks(slotItems, slotable.GetSlotQuantity());
				if (slotItems.Count > 0)
					DrawBonus();
			}
			Reposition();
		}

		private void DrawPerks(List<ItemSlot> slotItems, int slotQuantity)
		{
			// STUB: NGUI UITable rebuild needed
		}

		public void Clear()
		{
			foreach (Node child in perkTable.GetChildren())
			{
				child.QueueFree();
			}
			perkTable.Visible = false;
			if (bonusLabel != null)
				bonusLabel.Node.SetActive(false);
			if (bonusSprite != null)
				bonusSprite.Visible = false;
		}

		private Node DrawPerkCell(string name)
		{
			// STUB: NGUI UITexture + Color rebuild needed
			return CreatePerkCell(perkInfoPrf);
		}

		private void DrawBonus()
		{
			// STUB
		}

		private Node CreatePerkCell(Node prf)
		{
			// STUB: Object.Instantiate → PackedScene.Instantiate
			return null;
		}

		public void TintColor(Color col)
		{
			if (bonusLabel != null)
				bonusLabel.color = col;
		}

		private void Reposition()
		{
			// STUB: NGUI UITable.Reposition not available in Godot
		}
	}
}
