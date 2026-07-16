// SKELETON STUB: SF3/Items/ItemPerkCard.cs
// [MONO] Original used NGUI (Object.Instantiate, UIProgressBar).
// Needs full UI rebuild with Godot Control nodes.
using Godot;

namespace SF3.Items
{
	public partial class ItemPerkCard : Node
	{
		public Node3D itemCardPosition;

		public Node reelItemPrf;

		public Node3D progressPosition;

		public Node progressPrf;

		public NekkiUILabel itemNameLbl;

		public ItemSlotCtr itemSlotDrawer;

		private CardItem itemCard;

		private ProgressBar _progress;

		public void Init()
		{
			// STUB: NGUI Object.Instantiate → need PackedScene.Instantiate
			GD.PrintErr("ItemPerkCard.Init: STUB - NGUI rebuild needed");
		}

		public void SetItem(int id)
		{
			SetItem(UserManager.UserModelInfo.GetItemByID(id));
		}

		public void SetItem(BaseItem item)
		{
			SetBase(item);
			SetSlotable(item);
			SetEquipment(item);
		}

		public void SetPerkSelection(BaseItem item)
		{
			itemSlotDrawer.SetSelection(item as Perk);
		}

		private void SetBase(BaseItem item)
		{
			itemCard.Init(item);
			Visible = true;
			itemCard.UpdateShade(0f);
			itemNameLbl.Alias = item.alias;
			itemCard.SetImage();
		}

		private void SetSlotable(BaseItem item)
		{
			ISlotable slotable = item as ISlotable;
			if (slotable != null)
			{
				itemSlotDrawer.DrawSlots(slotable.GetSlotItems());
			}
		}

		private void SetEquipment(BaseItem item)
		{
			_progress.Visible = false;
			Equipment equipment = item as Equipment;
			if (equipment != null)
			{
				_progress.Visible = true;
				_progress.Value = equipment.GetBarValue() * 100f;
			}
		}
	}
}
