using System;
using Godot;

public partial class OptimizedGlow : Node
{
	[Export]
	public float glowIntensity = 1.0f;

	[Export]
	public Color glowColor = Colors.White;

	[Export]
	public float threshold = 0.5f;

	public override void _Ready()
	{
		GD.Print("STUB: OptimizedGlow._Ready");
	}

	public void SetActive(bool active)
	{
		Visible = active;
	}

	public void SetIntensity(float intensity)
	{
		glowIntensity = intensity;
	}
}
