using System;
using Godot;

public class TutorialBlockNGUI : Node
{
	[Export]
	public bool blockAllInput = true;

	[Export]
	public Color blockColor = Colors.Black;

	public override void _Ready()
	{
		GD.Print("STUB: TutorialBlockNGUI._Ready");
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
