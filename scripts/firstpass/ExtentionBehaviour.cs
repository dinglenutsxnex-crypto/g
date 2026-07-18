using Godot;
using System;
using System.Collections.Generic;

public partial class ExtentionBehaviour : Node
{
	private EventListener<CallEventArgs, int> eventListener = new EventListener<CallEventArgs, int>();

	protected void Log(object message)
	{
		GD.Print(message);
	}

	protected void LogWarning(object message)
	{
		GD.PushWarning(message);
	}

	protected void LogError(object message)
	{
		GD.PrintErr(message);
	}

	protected void LogException(Exception ex)
	{
		GD.PrintErr(ex.Message);
	}

	protected void Invoke(Action action, float t)
	{
		GetTree().CreateTimer(t).Timeout += () => action();
	}

	/// <summary>
	/// Unity-style GetComponent: checks self, then direct children.
	/// </summary>
	public T GetComponent<T>() where T : class
	{
		if (this is T self) return self;
		foreach (Node child in GetChildren())
			if (child is T c) return c;
		return null;
	}

	/// <summary>
	/// Unity-style GetComponentsInChildren: populates list with all matching nodes in the subtree.
	/// </summary>
	public void GetComponentsInChildren<T>(List<T> results) where T : Node
	{
		CollectDescendants(this, results);
	}

	private static void CollectDescendants<T>(Node parent, List<T> results) where T : Node
	{
		foreach (Node child in parent.GetChildren())
		{
			if (child is T t) results.Add(t);
			CollectDescendants(child, results);
		}
	}

	public override void _ExitTree()
	{
		eventListener.removeAllEventListener();
	}

	public void addEventListener(int evt, Action<CallEventArgs> callback)
	{
		eventListener.addEventListener(evt, callback);
	}

	public void addEventListener(int[] evt, Action<CallEventArgs> callback)
	{
		eventListener.addEventListener(evt, callback);
	}

	public void removeEvent(int evt)
	{
		eventListener.removeEvent(evt);
	}

	public void removeEventListener(int evt, Action<CallEventArgs> callback)
	{
		eventListener.removeEventListener(evt, callback);
	}

	public void callEvent(int evt, object content = null)
	{
		eventListener.callEvent(evt, content);
	}
}
