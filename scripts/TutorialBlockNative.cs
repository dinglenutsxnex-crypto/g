using System;
using Godot;

public class TutorialBlockNative : Node
{
	[Export]
	public bool blockAllInput = true;

	[Export]
	public Color blockColor = new Color(0f, 0f, 0f, 0.7f);

	public override void _Ready()
	{
		GD.Print("STUB: TutorialBlockNative._Ready");
	}

	public void ShowBlock()
	{
		Visible = true;
	}

	public void HideBlock()
	{
		Visible = false;
	}
}
