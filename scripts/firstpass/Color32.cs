using Godot;

/// <summary>
/// Unity Color32 compatibility struct. Stores RGBA as bytes, converts to/from Godot Color.
/// </summary>
public struct Color32
{
	public byte r, g, b, a;

	public Color32(byte r, byte g, byte b, byte a = 255)
	{
		this.r = r; this.g = g; this.b = b; this.a = a;
	}

	public static implicit operator Color(Color32 c) =>
		new Color(c.r / 255f, c.g / 255f, c.b / 255f, c.a / 255f);

	public static implicit operator Color32(Color c) =>
		new Color32((byte)(c.R * 255), (byte)(c.G * 255), (byte)(c.B * 255), (byte)(c.A * 255));
}
