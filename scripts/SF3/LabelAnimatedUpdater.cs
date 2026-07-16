using System;
using Godot;

public class LabelAnimatedUpdater : Node
{
	[Export]
	public Label targetLabel;

	[Export]
	public float animationDuration = 0.5f;

	private string _currentText;

	public override void _Ready()
	{
		GD.Print("STUB: LabelAnimatedUpdater._Ready");
	}

	public void UpdateText(string newText)
	{
		GD.Print("STUB: LabelAnimatedUpdater.UpdateText: " + newText);
		if (targetLabel != null)
			targetLabel.Text = newText;
	}

	public void UpdateTextAnimated(string newText)
	{
		GD.Print("STUB: LabelAnimatedUpdater.UpdateTextAnimated: " + newText);
	}
}
