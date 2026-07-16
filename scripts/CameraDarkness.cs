using System;
using Godot;

public class CameraDarkness : Node
{
	[Export]
	public Color darknessColor = Colors.Black;

	[Export]
	public float darknessAlpha = 0.0f;

	[Export]
	public float fadeSpeed = 1.0f;

	private ColorRect _darknessOverlay;

	public override void _Ready()
	{
		GD.Print("STUB: CameraDarkness._Ready");
	}

	public void SetDarkness(float alpha)
	{
		darknessAlpha = Mathf.Clamp(alpha, 0f, 1f);
		GD.Print("STUB: CameraDarkness.SetDarkness: " + alpha);
	}

	public void FadeIn(float duration = -1f)
	{
		float dur = duration > 0f ? duration : fadeSpeed;
		GD.Print("STUB: CameraDarkness.FadeIn: " + dur);
	}

	public void FadeOut(float duration = -1f)
	{
		float dur = duration > 0f ? duration : fadeSpeed;
		GD.Print("STUB: CameraDarkness.FadeOut: " + dur);
	}
}
