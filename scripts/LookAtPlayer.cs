using System;
using Godot;

public class LookAtPlayer : Node
{
	[Export]
	public Node3D target;

	[Export]
	public bool invertX;

	public override void _Ready()
	{
		GD.Print("STUB: LookAtPlayer._Ready");
	}

	public override void _Process(double delta)
	{
		GD.Print("STUB: LookAtPlayer._Process");
	}

	public void SetTarget(Node3D newTarget)
	{
		target = newTarget;
	}
}
