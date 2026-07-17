using System;
using System.Linq;
using Godot;
using Color = Godot.Color;
using sf3DTO;

namespace SF3.Settings
{
	public partial class ItemSettings : Node
	{
		[Serializable]
		private partial class ItemsRaritySettings
		{
			public Rarity rarity = Rarity.Common;
			public string cardBorder = string.Empty;
			public string descriptionLine = string.Empty;
			public string alias = string.Empty;
			public string spriteName = string.Empty;
			public Color color = Colors.White;
			public string boosterSpriteName = string.Empty;
			public Color boosterTextColor = Colors.White;
		}

		[Serializable]
		private partial class ItemsFactionSettings
		{
			public Faction faction;
			public string alias = string.Empty;
		}

		private ItemsRaritySettings[] _itemsRaritySettings;
		private ItemsFactionSettings[] _itemsFactionSettings;

		public Color lowerAttributeColor;
		public Color higherAttributeColor;
		public Color normalAttributeColor;
		public Color upgradeAttributeColor;
		public Color upgradeArrowAttributeColor;

		private ItemsRaritySettings GetRaritySettings(Rarity rarity)
		{
			ItemsRaritySettings itemsRaritySettings = _itemsRaritySettings.FirstOrDefault((ItemsRaritySettings rarityValue) => rarityValue.rarity == rarity);
			if (itemsRaritySettings == null)
			{
				GD.PrintErr(string.Format("Havnt any item settings for [{0}] rarity", rarity.ToString()));
				return new ItemsRaritySettings();
			}
			return itemsRaritySettings;
		}

		public string GetRarityCardBorder(Rarity rarity)
		{
			return GetRaritySettings(rarity).cardBorder;
		}

		public string GetRarityDescriptionLine(Rarity rarity)
		{
			return GetRaritySettings(rarity).descriptionLine;
		}

		public string GetRarityAlias(Rarity rarity)
		{
			return GetRaritySettings(rarity).alias;
		}

		public string GetRaritySpriteName(Rarity rarity)
		{
			return GetRaritySettings(rarity).spriteName;
		}

		public Color GetRarityColor(Rarity rarity)
		{
			return GetRaritySettings(rarity).color;
		}

		public string GetBoosterSpriteName(Rarity rarity)
		{
			return GetRaritySettings(rarity).boosterSpriteName;
		}

		public Color GetBoosterTextColor(Rarity rarity)
		{
			return GetRaritySettings(rarity).boosterTextColor;
		}

		public string GetFactionAlias(Faction faction)
		{
			string text = string.Empty;
			ItemsFactionSettings[] itemsFactionSettings = _itemsFactionSettings;
			foreach (ItemsFactionSettings itemsFactionSettings2 in itemsFactionSettings)
			{
				if (itemsFactionSettings2.faction == faction)
				{
					text = itemsFactionSettings2.alias;
					break;
				}
			}
			if (text.Length == 0)
			{
				GD.PrintErr(string.Format("Havn't any item settings for [{0}] faction", faction.ToString()));
			}
			return text;
		}
	}
}
