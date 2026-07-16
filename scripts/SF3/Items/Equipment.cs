using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Godot;
using Nekki;
using Nekki.Yaml;
using SF3.Settings;
using SF3.UserData;
using SimpleJSON;
using sf3DTO;

namespace SF3.Items
{
	public class Equipment : BattleItem, IEquipment, ICloneable, ISlotable, IAttributable, IRarable, IFactionable, IInformable, IStackable
	{
		private Faction _faction;

		private Rarity _rarity;

		private EquipmentType _typeEquipment;

		private string _enemyShadowMark;

		private string _shadowMark;

		private bool _hidden;

		private bool _isDefault;

		private AttributesHolder _attributesHolder;

		private ItemSlot[] _slots;

		private bool _isNew;

		private bool _equipped;

		private double _stackLevel;

		public int level { get; protected set; }

		public string EnemyShadowMark
		{
			get
			{
				return _enemyShadowMark;
			}
			set
			{
				_enemyShadowMark = value;
			}
		}

		public string ShadowMark
		{
			get
			{
				return _shadowMark;
			}
			set
			{
				_shadowMark = value;
			}
		}

		public double stackLevel
		{
			get
			{
				return _stackLevel;
			}
		}

		public Equipment(string key, string value)
			: base(key, value)
		{
			_isNew = false;
			_equipped = false;
			level = 0;
			_faction = Faction.UnknownFaction;
			_rarity = Rarity.Common;
			_typeEquipment = EquipmentType.None;
			_slots = new ItemSlot[0];
			ShadowMark = string.Empty;
			EnemyShadowMark = string.Empty;
			_hidden = false;
			_isDefault = false;
			_attributesHolder = new AttributesHolder(_typeEquipment);
			SetStackLevel(0.0);
		}

		public static Equipment Create(int itemID)
		{
			return ItemsManager.GetEquipmentById(itemID);
		}

		public static Equipment Create(Mapping itemData)
		{
			Equipment equipment = Create(int.Parse(itemData.GetText("ID").text));
			if (equipment != null)
			{
				equipment.FillData(itemData);
			}
			return equipment;
		}

		public static Equipment Create(Item equipmentData)
		{
			Equipment equipment = Create(equipmentData.ModelId);
			equipment.SetEquipped(equipmentData.Equipped);
			equipment.SetStackLevel(equipmentData.StackLevel);
			if (equipmentData.Perks != null)
			{
				equipment.SetPerks(equipmentData.Perks.ToList());
			}
			return equipment;
		}

		private void SetPerks(List<sf3DTO.PerkSlot> perks)
		{
			ClearPerks();
			if (perks == null) return;
			for (int i = 0; i < perks.Count; i++)
			{
				Perk perk = Perk.Create(perks[i].PerkModelId);
				if (perk != null && perks[i].SlotIndex < _slots.Length)
				{
					_slots[perks[i].SlotIndex].SetPerk(perk);
				}
			}
		}

		public static List<BaseItem> Create(List<Item> equipmentsData)
		{
			List<BaseItem> list = new List<BaseItem>();
			foreach (Item equipmentsDatum in equipmentsData)
			{
				list.Add(Create(equipmentsDatum));
			}
			return list;
		}

		public void ClearPerks()
		{
			for (int i = 0; i < _slots.Length; i++)
			{
				_slots[i].RemovePerk();
			}
		}

		private void FillData(Mapping itemData)
		{
			_isNew = false;
			Sequence sequence = itemData.GetSequence("Perks");
			if (sequence != null)
			{
				foreach (Mapping item in sequence.nodesInside)
				{
					int num = int.Parse(item.GetText("SlotIndex").text);
					if (num < _slots.Length)
					{
						Perk perk;
						ItemsManager.TryGetPerkById(int.Parse(item.GetText("ID").text), out perk);
						if (perk != null)
						{
							perk.SetStackLevel(double.Parse(item.GetText("StackLevel").text));
							_slots[num].SetPerk(perk);
						}
					}
				}
			}
			Scalar text = itemData.GetText("Equipped");
			if (text != null)
			{
				_equipped = text.text.Equals("1");
			}
			text = itemData.GetText("StackLevel");
			if (text != null)
			{
				SetStackLevel(double.Parse(text.text));
			}
		}

		public override List<YamlNode> ToYaml()
		{
			List<YamlNode> list = new List<YamlNode>();
			list.Add(new Scalar("ID", base.id.ToString()));
			list.Add(new Scalar("StackLevel", _stackLevel.ToString(CultureInfo.InvariantCulture)));
			list.Add(new Scalar("Equipped", _equipped ? "1" : "0"));
			if (_slots != null && _slots.Any((ItemSlot slot) => slot.GetPerk() != null))
			{
				List<YamlNode> list2 = new List<YamlNode>();
				foreach (ItemSlot slot2 in _slots)
				{
					Perk perk = slot2.GetPerk();
					if (perk != null)
					{
						list2.Add(new Mapping("Perk", new List<YamlNode>
						{
							new Scalar("SlotIndex", slot2.GetSlotIndex().ToString()),
							new Scalar("ID", perk.id.ToString()),
							new Scalar("StackLevel", perk.stackLevel.ToString(CultureInfo.InvariantCulture))
						}));
					}
				}
				list.Add(new Sequence("Perks", list2));
			}
			return list;
		}

		public override JSONClass ToJSON()
		{
			JSONClass jSONClass = new JSONClass();
			jSONClass.Add("ID", new JSONData(base.id));
			jSONClass.Add("StackLevel", new JSONData(_stackLevel));
			jSONClass.Add("Equipped", new JSONData(_equipped));
			return jSONClass;
		}

		public override object Clone()
		{
			Equipment equipment = (Equipment)MemberwiseClone();
			if (_slots != null)
			{
				equipment._slots = new ItemSlot[_slots.Length];
				for (int i = 0; i < _slots.Length; i++)
				{
					equipment._slots[i] = _slots[i].Clone();
				}
			}
			return equipment;
		}

		public Faction GetFactionType()
		{
			return _faction;
		}

		public Rarity GetRarityType()
		{
			return _rarity;
		}

		public EquipmentType GetEquipmentType()
		{
			return _typeEquipment;
		}

		public bool IsNew()
		{
			return _isNew;
		}

		public void SetNew(bool isNew)
		{
			_isNew = isNew;
		}

		public bool IsEquipped()
		{
			return _equipped;
		}

		public void SetEquipped(bool equipped)
		{
			_equipped = equipped;
		}

		public double GetStackLevel()
		{
			return _stackLevel;
		}

		public void SetStackLevel(double value)
		{
			_stackLevel = value;
		}

		public ItemSlot[] GetSlots()
		{
			return _slots;
		}

		public bool IsDefault()
		{
			return _isDefault;
		}
	}
}
