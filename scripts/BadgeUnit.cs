using System;
using Godot;

public class BadgeUnit : Node
{
	[Export]
	public TextureRect badgeIcon;

	[Export]
	public Label badgeName;

	[Export]
	public Label badgeDescription;

	public override void _Ready()
	{
		GD.Print("STUB: BadgeUnit._Ready");
	}

	public void SetData(string name, string description, Texture2D iconTexture = null)
	{
		if (badgeName != null) badgeName.Text = name;
		if (badgeDescription != null) badgeDescription.Text = description;
		if (badgeIcon != null && iconTexture != null) badgeIcon.Texture = iconTexture;
	}
}
