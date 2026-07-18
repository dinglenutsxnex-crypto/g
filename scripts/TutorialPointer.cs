using System;
using Godot;

public partial class TutorialPointer : Node
{
	[Export]
	public TextureRect pointerImage;

	[Export]
	public float animationSpeed = 1.0f;

	[Export]
	public Vector2 offset = Vector2.Zero;

	public override void _Ready()
	{
		GD.Print("STUB: TutorialPointer._Ready");
	}

	public void PointAt(Node target)
	{
		GD.Print("STUB: TutorialPointer.PointAt");
	}

	public void PointAtPosition(Vector2 screenPosition)
	{
		GD.Print("STUB: TutorialPointer.PointAtPosition");
	}

	public void Hide()
	{
		Visible = false;
	}

	public void Show()
	{
		Visible = true;
	}
}
