using System;
using Godot;

public partial class InventoryButton : Node
{
	[Export]
	public Button button;

	[Export]
	public Label label;

	[Export]
	public TextureRect icon;

	[Export]
	public Node notificationBadge;

	private object _itemData;

	public override void _Ready()
	{
		GD.Print("STUB: InventoryButton._Ready");
	}

	public void Init(object itemData)
	{
		_itemData = itemData;
		GD.Print("STUB: InventoryButton.Init");
	}

	public void SetNotification(bool hasNotification)
	{
		if (notificationBadge != null)
			notificationBadge.Visible = hasNotification;
	}

	public void SetSelected(bool selected)
	{
		GD.Print("STUB: InventoryButton.SetSelected: " + selected);
	}
}
