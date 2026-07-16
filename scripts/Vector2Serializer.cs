using System;
using Godot;

[Serializable]
public struct Vector2Serializer
{
	public float x;

	public float y;

	public Vector2 V2
	{
		get
		{
			return new Vector2(x, y);
		}
	}

	public Vector2Serializer(Vector2 v3)
	{
		x = v3.X;
		y = v3.Y;
	}

	public static implicit operator Vector2(Vector2Serializer v)
	{
		return v.V2;
	}
}
