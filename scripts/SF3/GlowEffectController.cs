using System;
using Godot;

public partial class GlowEffectController : Node
{
	[Export]
	public Color glowColor = Colors.Yellow;

	[Export]
	public float glowIntensity = 1.0f;

	public override void _Ready()
	{
		GD.Print("STUB: GlowEffectController._Ready");
	}

	public void SetGlow(bool enabled)
	{
		GD.Print("STUB: GlowEffectController.SetGlow: " + enabled);
	}

	public void SetColor(Color color)
	{
		glowColor = color;
	}

	public void SetIntensity(float intensity)
	{
		glowIntensity = intensity;
	}
}
