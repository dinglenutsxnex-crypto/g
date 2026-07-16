using System;
using Godot;

public class StickHelper : Node
{
	[Export]
	public float deadZone = 0.2f;

	[Export]
	public float sensitivity = 1.0f;

	public Vector2 GetInput()
	{
		GD.Print("STUB: StickHelper.GetInput");
		return Vector2.Zero;
	}

	public bool IsActive()
	{
		GD.Print("STUB: StickHelper.IsActive");
		return false;
	}

	public void Vibrate(float duration = 0.1f)
	{
		GD.Print("STUB: StickHelper.Vibrate");
	}
}
