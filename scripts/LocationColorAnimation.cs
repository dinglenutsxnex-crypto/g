using System;
using Godot;

public partial class LocationColorAnimation : Node
{
	[Export]
	public Color targetColor = Colors.White;

	[Export]
	public float animationDuration = 1.0f;

	public override void _Ready()
	{
		GD.Print("STUB: LocationColorAnimation._Ready");
	}

	public void AnimateToColor(Color color)
	{
		targetColor = color;
		GD.Print("STUB: LocationColorAnimation.AnimateToColor");
	}

	public void SetColor(Color color)
	{
		targetColor = color;
	}
}
