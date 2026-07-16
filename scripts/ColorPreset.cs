using System;
using Godot;

[Serializable]
public partial class ColorPreset
{
	public int ID;
	public Color color;
	public int minSaturation;
	public int maxSaturation = 255;

	public Color GetColor(float value)
	{
		Color col = color;
		float h = 0f, s = 0f, v = 0f;
		h = col.H;
		s = col.S;
		v = col.V;
		s = (value * (float)(maxSaturation - minSaturation) + (float)minSaturation) / 255f;
		return Color.FromHsv(h, s, v);
	}
}
