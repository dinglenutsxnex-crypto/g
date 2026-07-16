// SKELETON STUB: SF3/Items/ItemSlotCtr.cs
// [MONO] Original used NGUI (UIButton, NekkiUISprite, EventDelegate).
// Needs full UI rebuild with Godot Control nodes.
using System;
using Nekki.UI;
using System.Collections.Generic;
using Godot;
using Nekki.UI;

namespace SF3.Items
{
	public partial class ItemSlotCtr : Node
	{
		public delegate void ItemSelectedEventHandler(PerkSlot slot);

		public Node slotPrf;

		public float slotOffset = 10f;

		public ItemInsertEventHandler onInsertedItem;

		public ItemRemoveEventHandler onRemovedItem;

		public ItemChangeEventHandler onChangedItem;

		private NekkiUISprite _slotSprite;

		private int _selected;

		private bool _quiet;

		private List<PerkSlot> _slotsObj = new List<PerkSlot>();

		public ItemSelectedEventHandler onSelected;

		public PerkSlot selected_slot
		{
			get
			{
				if (_selected < 0 || _selected >= _slotsObj.Count)
					return null;
				return _slotsObj[_selected];
			}
		}

		public int selected_id
		{
			get
			{
				if (_slotsObj == null || _selected < 0 || _selected >= _slotsObj.Count)
					return -1;
				return _slotsObj[_selected].id;
			}
		}

		public int selected
		{
			get { return _selected; }
			set
			{
				foreach (PerkSlot item in _slotsObj)
				{
					item.Selected = item.index == value;
				}
				_selected = value;
				if (!_quiet && onSelected != null)
					onSelected(_slotsObj[_selected]);
				_quiet = false;
			}
		}

		public void DrawSlots(List<ItemSlot> slotItems)
		{
			// STUB: NGUI UIButton + EventDelegate + Object.Instantiate → need Godot Control rebuild
			GD.PrintErr("ItemSlotCtr.DrawSlots: STUB - NGUI rebuild needed");
		}

		private void OnSlotClick(PerkSlot slot)
		{
			// STUB
		}

		public PerkSlot GetSlotWithPerk(Perk perk)
		{
			foreach (PerkSlot item in _slotsObj)
			{
				if (item != null && item.perk != null && item.perk.id.Equals(perk.id))
					return item;
			}
			return null;
		}

		public void SetSelection(Perk perk)
		{
			if (perk == null || _slotsObj.Count <= 1)
				return;
			foreach (PerkSlot item in _slotsObj)
			{
				if (item.IsPerkType(perk))
				{
					SelectSlotQuiet(item.index);
					return;
				}
			}
			if (selected_slot.Empty)
				return;
			foreach (PerkSlot item2 in _slotsObj)
			{
				if (item2.Empty)
				{
					SelectSlotQuiet(item2.index);
					break;
				}
			}
		}

		private void SelectSlotQuiet(int index)
		{
			_quiet = true;
			selected = index;
		}

		public PerkSlot GetSlot()
		{
			if (selected >= _slotsObj.Count)
				return null;
			return _slotsObj[selected];
		}

		public bool HasSelectedSlotPerkOfType<T>(T item) where T : BaseItem
		{
			return _slotsObj[selected].IsPerkType(item);
		}
	}
}
