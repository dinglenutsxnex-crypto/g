using System;
using Nekki.Yaml;

/// <summary>
/// Helper methods for reading typed values out of Nekki.Yaml nodes.
/// </summary>
public static class YamlUtils
{
	public static Nekki.Yaml.Node GetNode(Nekki.Yaml.Node parent, string key)
	{
		if (parent is Mapping m) return m.GetNode(key);
		return null;
	}

	public static string GetText(Nekki.Yaml.Node node)
	{
		return node?.GetValue() ?? string.Empty;
	}

	public static bool TryGetString(Nekki.Yaml.Node node, string key, out string result)
	{
		result = string.Empty;
		var n = GetNode(node, key);
		if (n == null) return false;
		result = n.GetValue() ?? string.Empty;
		return true;
	}

	public static bool TryGetInt(Nekki.Yaml.Node node, string key, out int result)
	{
		result = 0;
		var n = GetNode(node, key);
		if (n == null) return false;
		return int.TryParse(n.GetValue(), out result);
	}

	public static bool TryGetFloat(Nekki.Yaml.Node node, string key, out float result)
	{
		result = 0f;
		var n = GetNode(node, key);
		if (n == null) return false;
		return float.TryParse(n.GetValue(), System.Globalization.NumberStyles.Float,
			System.Globalization.CultureInfo.InvariantCulture, out result);
	}
}
