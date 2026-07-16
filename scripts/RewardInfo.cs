// ⚠️ STUB: needs full port — original used NGUI NekkiUILabel, NekkiUILayout, UIProgressBar, Object.Instantiate
using System.Collections.Generic;
using SF3;
using SF3.Items;
using SF3_Attributes;
using Godot;

public partial class RewardInfo : Control
{
	[Export]
	private Label itemName;
	[Export]
	private Node progressBarPrf;
	[Export]
	private Control progressBarPlaceholder;
	[Export]
	private AttributesDrawer attributeDrawer;
	[Export]
	private Label description;

	public void Init()
	{
		HideProgress();
		HideAttributes();
		HideItemName();
	}

	public void ShowItemName(string nameAlias)
	{
		itemName.Text = nameAlias;
		itemName.Visible = true;
	}

	public void HideItemName()
	{
		itemName.Visible = false;
	}

	public void HideProgress()
	{
	}

	public void HideAttributes()
	{
		attributeDrawer.Visible = false;
	}

	public void ShowAttributes(SortedDictionary<AttributeType, float> attrs)
	{
		attributeDrawer.Draw(attrs);
	}

	public void ShowAttributes(SortedDictionary<AttributeType, float> attrs, SortedDictionary<AttributeType, float> equippedAttr)
	{
		attributeDrawer.Draw(attrs, equippedAttr);
	}

	public void HideDescription()
	{
		description.Visible = false;
	}

	public void ShowDescription(string alias)
	{
		description.Text = alias;
		description.Visible = true;
	}

	public void HideAll()
	{
		HideItemName();
		HideProgress();
		HideAttributes();
		HideDescription();
	}
}
