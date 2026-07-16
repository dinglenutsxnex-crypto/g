using System;
using Godot;

public partial class DebugButtonLineController : DebugLineController
{
	[Export]
	public Button button;

	[Export]
	private Label textLabel;

	private Action onClick;

	internal void setup(string text, Action _onClick)
	{
		onClick = _onClick;
		textLabel.Text = text;
	}

	public void onButtonPressed()
	{
		onClick();
	}
}
