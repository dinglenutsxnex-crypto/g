using System;
using Godot;

public partial class ReelItem : Node
{
	[Export]
	public TextureRect icon;

	[Export]
	public Label nameLabel;

	[Export]
	public Label rarityLabel;

	[Export]
	public Button clickButton;

	private object _data;

	public override void _Ready()
	{
		GD.Print("STUB: ReelItem._Ready");
	}

	public void Init(object itemData)
	{
		_data = itemData;
		GD.Print("STUB: ReelItem.Init");
	}

	public void SetHighlight(bool highlighted)
	{
		GD.Print("STUB: ReelItem.SetHighlight: " + highlighted);
	}

	public void PlayAppearAnimation()
	{
		GD.Print("STUB: ReelItem.PlayAppearAnimation");
	}

	public object GetData()
	{
		return _data;
	}
}
