using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// Manages long-lived singleton Nodes and fires application lifecycle events.
/// </summary>
public static class StaticObjectsManager
{
	private static readonly List<Node> _objects = new List<Node>();

	/// <summary>Fired when the OS suspends the app (mobile).</summary>
	public static event Action OnApplicationPauseEvent;
	/// <summary>Fired when the OS resumes the app (mobile).</summary>
	public static event Action OnApplicationResumeEvent;
	/// <summary>Fired when the app is about to quit.</summary>
	public static event Action OnApplicationQuitEvent;

	/// <summary>Register a Node to be kept alive as a static object.</summary>
	public static void AddObject(Node node)
	{
		if (node != null && !_objects.Contains(node))
			_objects.Add(node);
	}

	internal static void FirePause()   => OnApplicationPauseEvent?.Invoke();
	internal static void FireResume()  => OnApplicationResumeEvent?.Invoke();
	internal static void FireQuit()    => OnApplicationQuitEvent?.Invoke();
}
