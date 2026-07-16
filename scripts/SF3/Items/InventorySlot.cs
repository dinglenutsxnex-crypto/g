using System;
using SF3.Settings;
using Godot;
using sf3DTO;
namespace SF3.Items
{
	[Serializable]
	public partial class InventorySlot
	{
		[Export]
		private TextureRect _slotEmptyIcon;
		[Export]
		private TextureRect _itemInSlotBack;
		[Export]
		private EquipmentType _TypeEquipment;
		[Export]
		private TextureRect _CategoryAlert;
		[Export]
		private TextureRect _CategoryLock;
		[Export]
		private Node _perkInfo;
		[Export]
		private Node _perkInfoMarker;
		[Export]
		private TextureRect _itemTexture;
		[Export]
		private TextureRect _rarityBar;
		private Color _fullColorBg = new Color32(242, 233, 218, byte.MaxValue);
		private Color _fullDarkenColor = new Color32(180, 180, 180, byte.MaxValue);
		private Color _fullDarkenColorBg = new Color32(195, 191, 183, byte.MaxValue);
		private Color _emptyColorBg = new Color32(16, 16, 16, byte.MaxValue);
		private Color _emptyLockColorIcon = new Color32(27, 27, 27, byte.MaxValue);
		private Color _emptyDarkenColorIcon = new Color32(90, 89, 87, byte.MaxValue);
		private Color _emptyColorIcon = new Color32(173, 172, 168, byte.MaxValue);
		private bool _isLock;
		private TextureRect _perkMarker;
		private TextureRect _slotEmpty;
		private bool _empty = true;
		private string _alias;
		public EquipmentType TypeEquipment
		{
			get
			{
				return _TypeEquipment;
			}
		}
		public TextureRect CategoryAlert
		{
			get
			{
				return _CategoryAlert;
			}
		}
		public TextureRect Texture
		{
			get
			{
				return _itemTexture;
			}
		}
		public void SlotEmpty(bool empty)
		{
			_empty = empty;
			_itemTexture.enabled = !empty;
			_slotEmptyIcon.Visible = empty;
			if (!empty)
			{
				_itemInSlotBack.Modulate = _fullColorBg;
				return;
			}
			_itemInSlotBack.Modulate = _emptyColorBg;
			_slotEmptyIcon.Modulate = ((!_isLock) ? _emptyColorIcon : _emptyLockColorIcon);
		}
		public void SetRarity(IRarable rarable)
		{
			if (rarable != null)
			{
				if (_empty)
				{
					_rarityBar.spriteName = GameSettings.ItemSettings.GetRaritySpriteName(Rarity.Common);
					return;
				}
				Rarity rarityType = rarable.GetRarityType();
				_rarityBar.spriteName = GameSettings.ItemSettings.GetRaritySpriteName(rarityType);
			}
		}
		public void UnsetRarity()
		{
			_rarityBar.spriteName = GameSettings.ItemSettings.GetRaritySpriteName(Rarity.Common);
		}
		public void DarkenSlot(bool darken = true)
		{
			if (!_isLock)
			{
				if (darken)
				{
					Draken(_fullDarkenColor, (!_empty) ? _fullDarkenColorBg : _emptyColorBg, _emptyDarkenColorIcon);
				}
				else
				{
					Draken(Color.white, (!_empty) ? _fullColorBg : _emptyColorBg, _emptyColorIcon);
				}
			}
		}
		private void Draken(Color itemTextureColor, Color itemInSlotBackColor, Color slotEmptyIconColor)
		{
			_itemTexture.Modulate = itemTextureColor;
			_itemInSlotBack.Modulate = itemInSlotBackColor;
			_slotEmptyIcon.Modulate = slotEmptyIconColor;
		}
		public void LockSlot(bool isLock)
		{
			_isLock = isLock;
			_CategoryLock.Visible = isLock;
			if (isLock)
			{
				_itemInSlotBack.Modulate = _emptyColorBg;
				_slotEmptyIcon.Modulate = _emptyLockColorIcon;
			}
			else
			{
				DarkenSlot();
			}
		}
		private TextureRect GetEmptySprite()
		{
			if (_slotEmpty == null)
			{
				_slotEmpty = _slotEmptyIcon.GetComponent<TextureRect>();
			}
			return _slotEmpty;
		}
		public void SetImage(string alias)
		{
			_alias = alias;
			_itemTexture.GetShaderParameter("texture_albedo") = GlobalLoad.GetLoadTexture2D("UI/Inventory/" + alias);
		}
		public void RefreshImage()
		{
			if (!string.IsNullOrEmpty(_alias) && !_empty)
			{
				_itemTexture.Visible = false;
				_itemTexture.GetShaderParameter("texture_albedo") = GlobalLoad.GetLoadTexture2D("UI/Inventory/" + _alias);
				_itemTexture.Visible = true;
			}
		}
	}
}


