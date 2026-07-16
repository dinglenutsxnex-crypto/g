using System;
using Godot;

public class SceneDebug : Node
{
	[Export]
	public bool showDebugInfo = true;

	[Export]
	public Label debugLabel;

	public override void _Ready()
	{
		GD.Print("STUB: SceneDebug._Ready");
	}

	public void Log(string message)
	{
		GD.Print("[SCENE_DEBUG] " + message);
		if (debugLabel != null)
			debugLabel.Text += message + "\n";
	}

	public void Clear()
	{
		if (debugLabel != null)
			debugLabel.Text = string.Empty;
	}
}
