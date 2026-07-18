using System;
using Godot;

public partial class TutorialComponent : Node
{
	[Export]
	public bool isEnabled = true;

	public void Play(string stepId)
	{
		GD.Print("STUB: TutorialComponent.Play: " + stepId);
	}

	public void Stop()
	{
		GD.Print("STUB: TutorialComponent.Stop");
	}

	public bool GetVisible()
	{
		return isEnabled;
	}

	public void SetVisible(bool visible)
	{
		Visible = visible;
	}
}
