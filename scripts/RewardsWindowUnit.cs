using System;
using Godot;

public partial class RewardsWindowUnit : Node
{
	[Export]
	public TextureRect icon;

	[Export]
	public Label nameLabel;

	[Export]
	public Label quantityLabel;

	public override void _Ready()
	{
		GD.Print("STUB: RewardsWindowUnit._Ready");
	}

	public void SetData(string name, int quantity, Texture2D iconTexture = null)
	{
		GD.Print("STUB: RewardsWindowUnit.SetData: " + name + " x" + quantity);
		if (nameLabel != null)
			nameLabel.Text = name;
		if (quantityLabel != null)
			quantityLabel.Text = "x" + quantity;
		if (icon != null && iconTexture != null)
			icon.Texture = iconTexture;
	}
}
