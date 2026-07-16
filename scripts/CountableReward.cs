using System;
using Godot;

public class CountableReward : Node
{
	[Export]
	public TextureRect icon;

	[Export]
	public Label nameLabel;

	[Export]
	public Label countLabel;

	[Export]
	public Node countHolder;

	public override void _Ready()
	{
		GD.Print("STUB: CountableReward._Ready");
	}

	public void SetData(string name, int count, Texture2D iconTexture = null)
	{
		if (nameLabel != null) nameLabel.Text = name;
		if (countLabel != null) countLabel.Text = "x" + count;
		if (icon != null && iconTexture != null) icon.Texture = iconTexture;
		if (countHolder != null) countHolder.Visible = count > 1;
	}
}
