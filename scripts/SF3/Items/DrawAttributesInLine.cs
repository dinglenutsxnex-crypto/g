using System;
using System.Collections.Generic;
using SF3_Attributes;
using Godot;

namespace SF3.Items
{
	[Serializable]
	public partial class DrawAttributesInLine
	{
		public partial class AttributeLabelDataForDrawing
		{
			public string baseAttributeValue;

			public bool attributeDifferent;

			public Color attributeColor;

			public string difference;

			public string differenceImageName;

			public AttributeLabelDataForDrawing(string baseValue)
			{
				attributeDifferent = false;
				baseAttributeValue = baseValue;
			}

			public void SetAttributeDifference(Color diffColor, string diffString, string diffImage)
			{
				attributeDifferent = true;
				attributeColor = diffColor;
				difference = diffString;
				differenceImageName = diffImage;
			}
		}

		private class AttributeLabelUnit
		{
			private List<TextureRect> images;

			private List<Label> text;

			private Dictionary<Control, int> objectsOrderWithWidth;

			private float space;

			private Control parent;

			public float width { get; private set; }

			public AttributeLabelUnit(float spaceBetweenElements, Control _parent)
			{
				space = spaceBetweenElements;
				images = new List<TextureRect>();
				text = new List<Label>();
				objectsOrderWithWidth = new Dictionary<Control, int>();
				width = 0f;
				parent = _parent;
			}

			public void AddSpacesToWidth()
			{
				if (space != 0f)
				{
					width += (float)(text.Count + images.Count - 1) * space;
				}
			}

			public void RescaleUnit(float scale)
			{
				width = 0f;
				foreach (Label item in text)
				{
					item.FontSize = Mathf.RoundToInt((float)item.FontSize * scale);
					UpdateWidth(item);
				}
				foreach (TextureRect image in images)
				{
					image.Size = new Vector2(Mathf.RoundToInt(image.Size.X * scale), Mathf.RoundToInt(image.Size.Y * scale));
					UpdateWidth(image);
				}
				AddSpacesToWidth();
			}

			private void UpdateWidth(Control c)
			{
				objectsOrderWithWidth[c] = (int)c.Size.X;
				width += c.Size.X;
			}

			public void RecalculateWidth()
			{
				width = 0f;
				foreach (Label item in text)
				{
					UpdateWidth(item);
				}
				foreach (TextureRect image in images)
				{
					UpdateWidth(image);
				}
				AddSpacesToWidth();
			}

			public void AddLabel(Label lbl, int lblWidth)
			{
				Reparent(lbl, parent);
				text.Add(lbl);
				width += lblWidth;
				objectsOrderWithWidth[lbl] = lblWidth;
			}

			public void AddLabel(Label lbl)
			{
				AddLabel(lbl, (int)lbl.Size.X);
			}

			public void AddImage(TextureRect image, float scale = 1f)
			{
				AddImage(image, (int)image.Size.X, scale);
			}

			public void AddImage(TextureRect image, int imgWidth, float scale = 1f)
			{
				Reparent(image, parent);
				images.Add(image);
				width += Mathf.RoundToInt((float)imgWidth * scale);
				objectsOrderWithWidth[image] = Mathf.RoundToInt((float)imgWidth * scale);
			}

			private void Reparent(Control child, Control newParent)
			{
				if (child.GetParent() != newParent)
				{
					child.Reparent(newParent);
				}
			}

			public Vector3 SetupFromPosition(Vector3 pos)
			{
				Vector3 vector = pos;
				foreach (KeyValuePair<Control, int> item in objectsOrderWithWidth)
				{
					vector.X += (float)item.Value / 2f;
					item.Key.Position = vector;
					vector.X += (float)item.Value / 2f + space;
				}
				vector.X -= space;
				return vector;
			}
		}

		[Serializable]
		public partial class AttributeLblProperties
		{
			public Vector2Int iconSize;

			public Vector2Int spacing;

			public int fontSize;

			public Control parent;
		}

		[Serializable]
		public enum PivotType
		{
			Center,
			TopLeft,
			Top,
			TopRight,
			Left,
			Right,
			BottomLeft,
			Bottom,
			BottomRight
		}

		public enum OverflowBehavior
		{
			ShrinkContent,
			ClampContent,
			Overflow
		}

		public partial class LblProperties
		{
			public Vector2Int lblSize;

			public OverflowBehavior overflow;

			public PivotType pivot;

			public int fonSize;

			public Font font;
		}

		[Serializable]
		public partial class Vector2Int
		{
			public int x;
			public int y;
		}

		public Vector2Int arrowSize;

		public PivotType pivot;

		public PivotType attributeLinePivot = PivotType.Center;

		public AttributeLblProperties firstLblProperties;

		public bool skipFistLblProperties;

		public AttributeLblProperties normalLblProperties;

		public LblProperties attributeNameLblProp;

		private Label attributeNameLbl;

		private Label attributeNameLblSecondLine;

		public float maxLineWidth;

		public Color attributeColor;

		public float spaceBetweenUnits;

		public float spaceBetweenUnitElements;

		public int startCount;

		public Color statHigher;

		public Color statLower;

		public Color normalColor;

		private List<Label> labels;

		private List<TextureRect> sprites;

		public Texture2D imagesAtlas;

		public Font labelsFont;

		public Font trueTypeFont;

		private const string STATS_HIGHER_IMAGE = "arrowGreenStats";

		private const string STATS_LOWER_IMAGE = "arrowRedStats";

		private static Dictionary<AttributeType, string> atributeNameToImageParity = new Dictionary<AttributeType, string>
		{
			{ AttributeType.WeaponDamage, "weapon" },
			{ AttributeType.BodyDefense, "body" },
			{ AttributeType.HeadDefense, "head" },
			{ AttributeType.UnarmedDamage, "unarmed" },
			{ AttributeType.CriticalChance, "critical" },
			{ AttributeType.RangedDamage, "ranged" }
		};

		private static Dictionary<AttributeType, float> atributeNameToImageScale = new Dictionary<AttributeType, float>
		{
			{ AttributeType.WeaponDamage, 1f },
			{ AttributeType.BodyDefense, 1f },
			{ AttributeType.HeadDefense, 1f },
			{ AttributeType.UnarmedDamage, 0.65f },
			{ AttributeType.CriticalChance, 1f },
			{ AttributeType.RangedDamage, 1f }
		};

		private static Dictionary<AttributeType, string> attributeNameToStringFormat = new Dictionary<AttributeType, string>
		{
			{ AttributeType.CriticalChance, "{0:#0.##%}" }
		};

		public void Init()
		{
			labels = new List<Label>();
			sprites = new List<TextureRect>();
			for (int i = 0; i < startCount; i++)
			{
				CreateLabel();
				CreateSprite();
			}
			attributeNameLbl = CreateLabel<Label>();
			attributeNameLblSecondLine = CreateLabel<Label>();
			attributeNameLblSecondLine.FontSize = 14;
			labels.Remove(attributeNameLbl);
			labels.Remove(attributeNameLblSecondLine);
			DisableAll();
		}

		private Label CreateLabel()
		{
			return CreateLabel<Label>();
		}

		private T CreateLabel<T>() where T : Label, new()
		{
			T val = new T();
			val.Name = "AttributeText";
			val.Visible = true;
			if (normalLblProperties?.parent != null)
				normalLblProperties.parent.AddChild(val);
			if (trueTypeFont != null)
			{
				val.AddThemeFontSizeOverride("font_size", 14);
			}
			val.MouseFilter = Control.MouseFilterEnum.Ignore;
			labels.Add(val);
			return val;
		}

		private TextureRect CreateSprite()
		{
			TextureRect textureRect = new TextureRect();
			textureRect.Name = "AttributeImage";
			textureRect.Visible = true;
			if (normalLblProperties?.parent != null)
				normalLblProperties.parent.AddChild(textureRect);
			textureRect.MouseFilter = Control.MouseFilterEnum.Ignore;
			sprites.Add(textureRect);
			return textureRect;
		}

		private void ApplyLblProperties(Label lbl, LblProperties properties)
		{
			lbl.Modulate = normalColor;
			if (properties != null)
			{
				ApplyLabelPivot(lbl, properties.pivot);
				lbl.FontSize = properties.fonSize;
				lbl.Size = new Vector2(properties.lblSize.x, properties.lblSize.y);
			}
		}

		private static void ApplyLabelPivot(Label lbl, PivotType pivot)
		{
			switch (pivot)
			{
				case PivotType.TopLeft: lbl.HorizontalAlignment = HorizontalAlignment.Left; lbl.VerticalAlignment = VerticalAlignment.Top; break;
				case PivotType.Top: lbl.HorizontalAlignment = HorizontalAlignment.Center; lbl.VerticalAlignment = VerticalAlignment.Top; break;
				case PivotType.TopRight: lbl.HorizontalAlignment = HorizontalAlignment.Right; lbl.VerticalAlignment = VerticalAlignment.Top; break;
				case PivotType.Left: lbl.HorizontalAlignment = HorizontalAlignment.Left; lbl.VerticalAlignment = VerticalAlignment.Center; break;
				case PivotType.Center: lbl.HorizontalAlignment = HorizontalAlignment.Center; lbl.VerticalAlignment = VerticalAlignment.Center; break;
				case PivotType.Right: lbl.HorizontalAlignment = HorizontalAlignment.Right; lbl.VerticalAlignment = VerticalAlignment.Center; break;
				case PivotType.BottomLeft: lbl.HorizontalAlignment = HorizontalAlignment.Left; lbl.VerticalAlignment = VerticalAlignment.Bottom; break;
				case PivotType.Bottom: lbl.HorizontalAlignment = HorizontalAlignment.Center; lbl.VerticalAlignment = VerticalAlignment.Bottom; break;
				case PivotType.BottomRight: lbl.HorizontalAlignment = HorizontalAlignment.Right; lbl.VerticalAlignment = VerticalAlignment.Bottom; break;
			}
		}

		public void DisableAll()
		{
			foreach (Label label in labels)
			{
				label.Visible = false;
			}
			foreach (TextureRect sprite in sprites)
			{
				sprite.Visible = false;
			}
			if (attributeNameLbl != null)
				attributeNameLbl.Visible = false;
			if (attributeNameLblSecondLine != null)
				attributeNameLblSecondLine.Visible = false;
		}

		private Label GetFreeLabel()
		{
			for (int i = 0; i < labels.Count; i++)
			{
				if (!labels[i].Visible)
				{
					labels[i].Visible = true;
					return labels[i];
				}
			}
			return CreateLabel();
		}

		private TextureRect GetFreeSprite()
		{
			for (int i = 0; i < sprites.Count; i++)
			{
				if (!sprites[i].Visible)
				{
					sprites[i].Visible = true;
					return sprites[i];
				}
			}
			return CreateSprite();
		}

		private void SetLabelsToPositions(List<AttributeLabelUnit> attributesLabels, bool vertical = false)
		{
			if (attributesLabels.Count <= 0)
				return;

			float num = 0f;
			float num2 = 0f;
			if (vertical)
			{
				num2 = 0f;
				for (int i = 0; i < attributesLabels.Count; i++)
				{
					if (num2 < attributesLabels[i].width || i == 0)
						num2 = attributesLabels[i].width;
				}
				num2 /= -2f;
			}
			else
			{
				num2 = (float)(attributesLabels.Count - 1) * spaceBetweenUnits;
				foreach (AttributeLabelUnit attributesLabel in attributesLabels)
				{
					num2 += attributesLabel.width;
				}
				if (num2 > maxLineWidth)
				{
					float scale = maxLineWidth / num2;
					num2 = (float)(attributesLabels.Count - 1) * spaceBetweenUnits;
					foreach (AttributeLabelUnit attributesLabel2 in attributesLabels)
					{
						attributesLabel2.RescaleUnit(scale);
						num2 += attributesLabel2.width;
					}
				}
			}
			Vector3 pos = Vector3.Zero;
			if (attributeLinePivot == PivotType.Center)
			{
				pos = new Vector3((0f - num2) / 2f, 0f, 0f);
			}
			for (int j = 0; j < attributesLabels.Count; j++)
			{
				if (vertical)
				{
					attributesLabels[j].SetupFromPosition(new Vector3(num2, num, 0f));
					num -= spaceBetweenUnits;
				}
				else
				{
					pos = attributesLabels[j].SetupFromPosition(pos);
					pos.X += spaceBetweenUnits;
				}
			}
		}

		public void ShowAttributes(BaseItem item, bool vertical = false)
		{
			DisableAll();
			if (item == null) return;

			List<AttributeLabelUnit> list = new List<AttributeLabelUnit>();
			Dictionary<AttributeType, float> allAtributes = new Dictionary<AttributeType, float>();
			CalculateAllAtributes(item, ref allAtributes);
			bool flag = true;
			foreach (AttributeType key in atributeNameToImageParity.Keys)
			{
				if (allAtributes.ContainsKey(key))
				{
					if (flag && !skipFistLblProperties)
					{
						flag = false;
						AttributeLabelUnit unit = CreateAttributeUnit(key, allAtributes, firstLblProperties, false);
						ApplyLblProperties(attributeNameLbl, attributeNameLblProp);
						attributeNameLbl.Text = key.ToString();
						attributeNameLbl.Visible = true;
						unit.AddLabel(attributeNameLbl, 0);
						unit.AddSpacesToWidth();
						SetLabelsToPositions(new List<AttributeLabelUnit> { unit }, vertical);
					}
					else
					{
						list.Add(CreateAttributeUnit(key, allAtributes, normalLblProperties, false));
					}
				}
			}
			SetLabelsToPositions(list, vertical);
		}

		public void ShowComparedAttributes(BaseItem equiped, BaseItem selected_, bool vertical = false, bool nameOnSecondLine = false)
		{
			DisableAll();
			Dictionary<AttributeType, float> allAtributes = new Dictionary<AttributeType, float>();
			CalculateAllAtributes(selected_, ref allAtributes);
			Dictionary<AttributeType, float> allAtributes2 = new Dictionary<AttributeType, float>();
			CalculateAllAtributes(equiped, ref allAtributes2, false);
			List<AttributeLabelUnit> list = new List<AttributeLabelUnit>();
			Dictionary<AttributeType, AttributeLabelDataForDrawing> dictionary = new Dictionary<AttributeType, AttributeLabelDataForDrawing>();

			Equipment equipment = selected_ as Equipment;
			if (equipment?.GetAttributesForCombat() == null)
				return;

			float num = 0f;
			foreach (KeyValuePair<AttributeType, float> item in allAtributes)
			{
				num = 0f;
				if (allAtributes2.ContainsKey(item.Key))
				{
					num = allAtributes2[item.Key];
					allAtributes2.Remove(item.Key);
				}
				string baseValue = string.Empty + item.Value;
				if (attributeNameToStringFormat.ContainsKey(item.Key))
					baseValue = string.Format(attributeNameToStringFormat[item.Key], item.Value);

				AttributeLabelDataForDrawing data = new AttributeLabelDataForDrawing(baseValue);
				double diff = GetAttributesDifference(item.Value, num);
				if (diff != 0.0)
				{
					baseValue = string.Empty + diff;
					if (attributeNameToStringFormat.ContainsKey(item.Key))
						baseValue = string.Format(attributeNameToStringFormat[item.Key], diff);
					data.SetAttributeDifference(GetAttributeColor(item.Value, num), baseValue, GetAttributeDifferenceImage(item.Value, num));
				}
				dictionary.Add(item.Key, data);
			}
			foreach (KeyValuePair<AttributeType, float> item2 in allAtributes2)
			{
				AttributeLabelDataForDrawing data2 = new AttributeLabelDataForDrawing("0");
				string diffString = string.Empty + item2.Value;
				if (attributeNameToStringFormat.ContainsKey(item2.Key))
					diffString = string.Format(attributeNameToStringFormat[item2.Key], item2.Value);
				data2.SetAttributeDifference(statLower, diffString, "arrowRedStats");
				dictionary.Add(item2.Key, data2);
			}

			bool flag = true;
			foreach (AttributeType key in atributeNameToImageParity.Keys)
			{
				if (dictionary.ContainsKey(key))
				{
					if (flag && !skipFistLblProperties)
					{
						flag = false;
						AttributeLabelUnit unit = CreateAttributeUnit(key, dictionary, firstLblProperties, false);
						ApplyLblProperties(attributeNameLbl, attributeNameLblProp);
						attributeNameLbl.Text = key.ToString();
						attributeNameLbl.Visible = true;
						unit.AddLabel(attributeNameLbl, 0);
						unit.AddSpacesToWidth();
						list.Add(unit);
					}
					else if (nameOnSecondLine)
					{
						nameOnSecondLine = false;
						AttributeLabelUnit unit2 = CreateAttributeUnit(key, dictionary, firstLblProperties, false);
						ApplyLblProperties(attributeNameLblSecondLine, attributeNameLblProp);
						attributeNameLblSecondLine.Text = key.ToString();
						attributeNameLblSecondLine.Visible = true;
						unit2.AddLabel(attributeNameLblSecondLine, 0);
						unit2.AddSpacesToWidth();
						list.Add(unit2);
					}
					else
					{
						list.Add(CreateAttributeUnit(key, dictionary, normalLblProperties));
					}
				}
			}
			SetLabelsToPositions(list, vertical);
		}

		private AttributeLabelUnit CreateAttributeUnit(AttributeType attributeName, Dictionary<AttributeType, AttributeLabelDataForDrawing> dataForDrawing, AttributeLblProperties lblProperties, bool addSpaces = true)
		{
			AttributeLabelUnit unit = new AttributeLabelUnit(spaceBetweenUnitElements, lblProperties.parent);
			TextureRect sprite = CreateSprite(atributeNameToImageParity[attributeName], lblProperties.iconSize, attributeColor);
			float num = ((float)sprite.Size.X * atributeNameToImageScale[attributeName] - (float)sprite.Size.X) * -1f;
			unit.AddImage(sprite, atributeNameToImageScale[attributeName]);
			Label label = CreateLabel(dataForDrawing[attributeName].baseAttributeValue, dataForDrawing[attributeName].attributeDifferent ? dataForDrawing[attributeName].attributeColor : normalColor, lblProperties);
			unit.AddLabel(label, Mathf.RoundToInt((float)label.Size.X + num));
			if (addSpaces)
				unit.AddSpacesToWidth();
			return unit;
		}

		private AttributeLabelUnit CreateAttributeUnit(AttributeType attributeName, Dictionary<AttributeType, float> attributesValues, AttributeLblProperties lblProperties, bool addSpaces = true)
		{
			AttributeLabelUnit unit = new AttributeLabelUnit(spaceBetweenUnitElements, lblProperties.parent);
			TextureRect sprite = CreateSprite(atributeNameToImageParity[attributeName], lblProperties.iconSize, attributeColor);
			float num = ((float)sprite.Size.X * atributeNameToImageScale[attributeName] - (float)sprite.Size.X) * -1f;
			unit.AddImage(sprite, atributeNameToImageScale[attributeName]);
			string text = attributesValues[attributeName].ToString();
			if (attributeNameToStringFormat.ContainsKey(attributeName))
				text = string.Format(attributeNameToStringFormat[attributeName], attributesValues[attributeName]);
			Label label = CreateLabel(text, normalColor, lblProperties);
			unit.AddLabel(label, Mathf.RoundToInt((float)label.Size.X + num));
			if (addSpaces)
				unit.AddSpacesToWidth();
			return unit;
		}

		private Label CreateLabel(string text, Color color, AttributeLblProperties lblProperties)
		{
			Label freeLabel = GetFreeLabel();
			freeLabel.FontSize = lblProperties.fontSize;
			freeLabel.Text = text;
			freeLabel.Modulate = color;
			return freeLabel;
		}

		private TextureRect CreateSprite(string attImage, Vector2Int size)
		{
			return CreateSprite(attImage, size, Colors.White);
		}

		private TextureRect CreateSprite(string attImage, Vector2Int size, Color color)
		{
			TextureRect freeSprite = GetFreeSprite();
			freeSprite.Modulate = color;
			freeSprite.Size = new Vector2(size.x, size.y);
			return freeSprite;
		}

		public string GetAttributeDifferenceImage(float selectedValue, float equippedValue)
		{
			float num = selectedValue - equippedValue;
			if (num < 0f) return "arrowRedStats";
			if (num > 0f) return "arrowGreenStats";
			return string.Empty;
		}

		public Color GetAttributeColor(float selectedValue, float equippedValue)
		{
			float num = selectedValue - equippedValue;
			if (num < 0f) return statLower;
			if (num > 0f) return statHigher;
			return normalColor;
		}

		public double GetAttributesDifference(float selectedValue, float equippedValue)
		{
			double num = Math.Round((double)selectedValue - (double)equippedValue, 3);
			if (num < 0.0) num *= -1.0;
			return num;
		}

		private void CalculateAllAtributes(BaseItem item, ref Dictionary<AttributeType, float> allAtributes, bool considerSlots = true)
		{
			Equipment equipment = item as Equipment;
			if (equipment == null) return;

			if (equipment.GetAttributesForCombat() != null)
			{
				foreach (KeyValuePair<AttributeType, float> attr in equipment.GetAttributesForCombatData())
				{
					if (!allAtributes.ContainsKey(attr.Key))
						allAtributes.Add(attr.Key, GetAttributeValue(item, attr));
					else
						allAtributes[attr.Key] += GetAttributeValue(item, attr);
				}
			}
			GD.PrintErr("Trying to use obsolete [itemsIn] field using old API");
		}

		private float GetAttributeValue(BaseItem item, KeyValuePair<AttributeType, float> attribute)
		{
			Equipment eq = item as Equipment;
			return eq == null ? attribute.Value : eq.GetAttributesForDisplayValue(attribute.Key);
		}
	}
}
