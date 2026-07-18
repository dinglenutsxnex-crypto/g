using System;
using System.Collections.Generic;
using Godot;
using SF3;
using SF3.Items;
using SF3.UserData;
using sf3DTO;

public partial class CardItem : Control
{
	public struct AnimationParams
	{
		public float animScaleValue;

		public float animScaleTime;
	}

	[Export]
	private TextureRect _equipSprite;

	[Export]
	private TextureRect _itemImage;

	[Export]
	private TextureRect _itemBack;

	[Export]
	private TextureRect _rarityStrip;

	[Export]
	private Label _nameLabel;

	[Export]
	private Label _lvlLabel;

	[Export]
	private Label _typeLabel;

	[Export]
	private Label _ratingLbl;

	[Export]
	private Node _lvlHolder;

	[Export]
	private Node _perksHolder;

	[Export]
	private Node _animPanel;

	[Export]
	private Node _defaultStage;

	[Export]
	private Node _currentStage;

	[Export]
	private Node _soundBtn;

	private AnimationParams _animParams;

	private BaseItem _data;

	private EquipmentType _currentType;

	private List<PerkUnit> _perkUnits = new List<PerkUnit>();

	private TextureRect _dynItemSprite;

	private static readonly Dictionary<EquipmentType, float> TypeToScale = new Dictionary<EquipmentType, float>
	{
		{ EquipmentType.Head, 3f },
		{ EquipmentType.Body, 2.1f },
		{ EquipmentType.Hands, 2.1f },
		{ EquipmentType.Legs, 2.1f },
		{ EquipmentType.Weapon, 2.6f }
	};

	public BaseItem ItemData
	{
		get
		{
			return _data;
		}
	}

	public void Init(BaseItem item)
	{
		if (item == null)
		{
			Visible = false;
			return;
		}
		_data = item;
		if (item is Equipment)
		{
			_currentType = ((Equipment)item).GetEquipmentType();
		}
	}

	public void SetItem()
	{
		if (_data == null)
		{
			return;
		}
		_nameLabel.Text = _data.Name;
		_typeLabel.Text = "type_" + _currentType;
		_lvlLabel.Text = _data.Level.ToString();
		_ratingLbl.Text = _data.Rating.ToString();
		_lvlHolder.Visible = true;
		SetRarity();
		_perksHolder.Visible = false;
		_currentStage = _defaultStage;
	}

	private void SetRarity()
	{
	}

	public void PlayHide()
	{
		_currentStage = _animPanel;
		AnimationStart();
	}

	public void PlayShow()
	{
		_currentStage = _defaultStage;
		AnimationStart();
	}

	public void AnimationStart()
	{
		Tween tween = CreateTween();
		float targetScale = _animParams.animScaleValue;
		tween.TweenProperty(this, "scale", new Vector2(targetScale, targetScale), _animParams.animScaleTime);
	}

	public void Focus()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(this, "scale", new Vector2(1f, 1f), _animParams.animScaleTime);
	}

	public void ChangeItemImage(string path)
	{
	}

	public void SetItemIcon(Texture2D icon, bool isRarity, Texture2D rarityTexture = null, Texture2D backTexture = null)
	{
		_itemImage.Texture = icon;
		if (isRarity && rarityTexture != null)
		{
			_itemBack.Texture = rarityTexture;
		}
		if (backTexture != null)
		{
			_itemBack.Texture = backTexture;
		}
	}

	public void SetItemName(string itemName)
	{
		_nameLabel.Text = itemName;
	}

	public void SetItemLvl(string lvl)
	{
		_lvlLabel.Text = lvl;
	}

	public void UpdatePerks()
	{
		if (_data != null && _data is Equipment)
		{
			Equipment equipment = _data as Equipment;
			List<ItemSlot> slotItems = equipment.GetSlotItems();
			if (slotItems.Count > 0)
			{
				_perksHolder.Visible = true;
			}
		}
	}

	private void UpdatePerkSlots()
	{
		foreach (PerkUnit perkUnit in _perkUnits)
		{
			perkUnit.QueueFree();
		}
		_perkUnits.Clear();
		if (_data != null && _data is Equipment)
		{
			Equipment equipment = _data as Equipment;
			List<ItemSlot> slotItems = equipment.GetSlotItems();
			foreach (ItemSlot item in slotItems)
			{
				if (item.HasPerk())
				{
					CreatePerkSlot(item);
				}
			}
		}
	}

	private void CreatePerkSlot(ItemSlot item)
	{
	}

	public void PlayCustomAnimation(float scaleValue, float time)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(this, "scale", new Vector2(scaleValue, scaleValue), time);
	}

	public void Play()
	{
	}

	public void Stop()
	{
	}
}
