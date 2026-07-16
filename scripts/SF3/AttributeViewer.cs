// ⚠️ STUB: needs full port — original used NGUI UISprite, NekkiUILabel
using System.Collections.Generic;
using SF3.Settings;
using SF3_Attributes;
using Godot;

namespace SF3
{
	public partial class AttributeViewer : Control
	{
		private static readonly Dictionary<AttributeType, string> AtributeNameToSpriteName = new Dictionary<AttributeType, string>
		{
			{ AttributeType.WeaponDamage, "weapon" },
			{ AttributeType.BodyDefense, "body" },
			{ AttributeType.HeadDefense, "head" },
			{ AttributeType.UnarmedDamage, "unarmed" },
			{ AttributeType.CriticalChance, "critical" },
			{ AttributeType.RangedDamage, "range" },
			{ AttributeType.MagicPower, "magic" },
			{ AttributeType.Cooldown, "cooldown" }
		};

		private static readonly List<AttributeType> AttributesInPercent = new List<AttributeType> { AttributeType.CriticalChance };

		[Export]
		private TextureRect iconSprite;
		[Export]
		private TextureRect arrowIcon;
		[Export]
		private Label valueLabel;

		public void SetAttribute(AttributeType iconName, float itemAttrVal)
		{
			SetAttribute(iconName, itemAttrVal, itemAttrVal);
		}

		public void SetAttribute(AttributeType iconName, float itemAttrVal, float playerAttrVal)
		{
			if (AttributesInPercent.Contains(iconName))
			{
				valueLabel.Text = itemAttrVal * 100f + "%";
			}
			else
			{
				valueLabel.Text = itemAttrVal.ToString();
			}
			arrowIcon.Visible = itemAttrVal != playerAttrVal;
			SetViewColor(itemAttrVal, playerAttrVal);
			SetArrowScale(itemAttrVal, playerAttrVal);
		}

		private void SetViewColor(float itemAttrVal, float playerAttrVal)
		{
			Color color = arrowIcon.Modulate;
			if (playerAttrVal > itemAttrVal)
			{
				color = GameSettings.ItemSettings.lowerAttributeColor;
			}
			else if (playerAttrVal < itemAttrVal)
			{
				color = GameSettings.ItemSettings.higherAttributeColor;
			}
			SetArrowColor(color);
			SetValueColor(color);
		}

		private void SetArrowScale(float itemAttrVal, float playerAttrVal)
		{
			Vector2 localScale = arrowIcon.Scale;
			localScale.Y = Mathf.Abs(localScale.Y);
			if (playerAttrVal > itemAttrVal)
			{
				localScale.Y *= -1f;
			}
			arrowIcon.Scale = localScale;
		}

		public void SetIconColor(Color color)
		{
			iconSprite.Modulate = color;
		}

		public void SetValueColor(Color color)
		{
			valueLabel.Modulate = color;
		}

		public void SetArrowColor(Color color)
		{
			arrowIcon.Modulate = color;
		}
	}
}
