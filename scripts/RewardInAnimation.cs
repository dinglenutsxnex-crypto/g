// ⚠️ STUB: needs full port — original used NGUI (UITexture, UIWidget), DOTween sequences, custom shader material animation
using Godot;
using System;

public partial class RewardInAnimation : Node
{
	public delegate void OnAnimationEnd();

	[Export]
	public float duration = 1f;
	[Export]
	public float fadeDuration = 0.3f;
	[Export]
	public Curve curve;

	public event OnAnimationEnd onAnimationEnd = delegate { };

	public void Play() { }
	public void Break() { }
}
