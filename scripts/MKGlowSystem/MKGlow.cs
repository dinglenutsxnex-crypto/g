using System;
using Godot;

namespace MKGlowSystem
{
	public class MKGlow : Node
	{
		[Export]
		public float glowIntensity = 1.0f;

		[Export]
		public Color glowColor = Colors.White;

		[Export]
		public float blurSize = 4.0f;

		public override void _Ready()
		{
			GD.Print("STUB: MKGlow._Ready");
		}

		public void SetGlowIntensity(float intensity)
		{
			glowIntensity = intensity;
		}

		public void SetGlowColor(Color color)
		{
			glowColor = color;
		}
	}
}
