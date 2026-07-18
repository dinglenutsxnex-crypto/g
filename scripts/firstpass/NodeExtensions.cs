using System.Collections.Generic;
using Godot;

/// <summary>
/// Unity-style extension helpers on Godot Node.
/// These let any Node subclass (Node3D, AnimationPlayer, etc.) call
/// GetComponent/GetComponentsInChildren without inheriting ExtentionBehaviour.
/// </summary>
public static class NodeExtensions
{
	/// <summary>
	/// Unity-style GetComponent: checks the node itself, then its direct children.
	/// </summary>
	public static T GetComponent<T>(this Node node) where T : class
	{
		if (node is T self) return self;
		foreach (Node child in node.GetChildren())
			if (child is T c) return c;
		return null;
	}

	/// <summary>
	/// Unity-style GetComponentsInChildren: fills <paramref name="results"/> with all
	/// descendant nodes of type T.
	/// </summary>
	public static void GetComponentsInChildren<T>(this Node node, List<T> results) where T : Node
	{
		CollectDescendants(node, results);
	}

	/// <summary>
	/// Unity-style GetComponentsInChildren: returns a new list of all descendant nodes of type T.
	/// </summary>
	public static List<T> GetComponentsInChildren<T>(this Node node) where T : Node
	{
		var list = new List<T>();
		CollectDescendants(node, list);
		return list;
	}

	private static void CollectDescendants<T>(Node parent, List<T> results) where T : Node
	{
		foreach (Node child in parent.GetChildren())
		{
			if (child is T t) results.Add(t);
			CollectDescendants(child, results);
		}
	}
}
