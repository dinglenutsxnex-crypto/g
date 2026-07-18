using Godot;
using System;

public partial class TestFightGUI : Node
{
	public HUD userHUD;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void AddHealth(string value)
	{
		float result;
		float.TryParse(value, out result);
		if (userHUD == null)
		{
		}
	}
}
