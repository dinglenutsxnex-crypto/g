using System;
using System.Collections.Generic;

/// <summary>
/// Utility for resolving enum values from string names or integer codes.
/// </summary>
public static class EnumsCompliancer
{
	private static readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

	public static void Clear() => _cache.Clear();

	/// <summary>
	/// Parse an enum value of type T from a string name or integer stored in a YAML node.
	/// Returns default(T) when parsing fails.
	/// </summary>
	public static T GetEnumerator<T>(string name) where T : struct, Enum
	{
		if (Enum.TryParse<T>(name, true, out var val)) return val;
		return default;
	}

	public static T GetEnumerator<T>(string name, out T result) where T : struct, Enum
	{
		result = GetEnumerator<T>(name);
		return result;
	}

	public static T GetEnumerator<T>(Nekki.Yaml.Node node, out T result) where T : struct, Enum
	{
		result = default;
		if (node == null) return result;
		return GetEnumerator<T>(node.GetValue(), out result);
	}

	public static T GetEnumerator<T>(Nekki.Yaml.Node node) where T : struct, Enum
	{
		if (node == null) return default;
		return GetEnumerator<T>(node.GetValue());
	}
}
