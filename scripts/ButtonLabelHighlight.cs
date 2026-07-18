using System;
using Godot;

public partial class ButtonLabelHighlight : Node
{
	[Export]
	public Button targetButton;

	[Export]
	public Label targetLabel;

	[Export]
	public Color normalColor = Colors.White;

	[Export]
	public Color highlightColor = Colors.Yellow;

	public void Highlight(bool highlighted)
	{
		if (targetLabel != null)
		{
			targetLabel.Modulate = highlighted ? highlightColor : normalColor;
		}
		GD.Print("STUB: ButtonLabelHighlight.Highlight: " + highlighted);
	}
}
