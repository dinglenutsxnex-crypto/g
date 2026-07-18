using System;
using Godot;

public partial class ItemCardDebugUI : Node
{
	[Export]
	public Label itemNameLabel;

	[Export]
	public Label itemIdLabel;

	[Export]
	public Label itemTypeLabel;

	public override void _Ready()
	{
		GD.Print("STUB: ItemCardDebugUI._Ready");
	}

	public void ShowItemInfo(object item)
	{
		GD.Print("STUB: ItemCardDebugUI.ShowItemInfo");
	}

	public void Clear()
	{
		if (itemNameLabel != null) itemNameLabel.Text = string.Empty;
		if (itemIdLabel != null) itemIdLabel.Text = string.Empty;
		if (itemTypeLabel != null) itemTypeLabel.Text = string.Empty;
	}
}
