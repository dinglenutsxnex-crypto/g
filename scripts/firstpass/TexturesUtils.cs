using System.Collections.Generic;
using Godot;

/// <summary>
/// Reference-counted texture cache to avoid duplicate GPU uploads.
/// </summary>
public static class TexturesUtils
{
	private static readonly Dictionary<Texture2D, int> _refCounts = new Dictionary<Texture2D, int>();

	public static void AddTexture(Texture2D texture)
	{
		if (texture == null) return;
		_refCounts.TryGetValue(texture, out int count);
		_refCounts[texture] = count + 1;
	}

	public static void ReleaseTexture(Texture2D texture)
	{
		if (texture == null) return;
		if (_refCounts.TryGetValue(texture, out int count))
		{
			if (count <= 1) _refCounts.Remove(texture);
			else _refCounts[texture] = count - 1;
		}
	}
}
