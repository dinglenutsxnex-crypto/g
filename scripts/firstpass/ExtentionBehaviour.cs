using Godot;
using System;

public class ExtentionBehaviour : Node
{
	private EventListener<CallEventArgs, int> eventListener = new EventListener<CallEventArgs, int>();

	public new Node Name { get; set; }

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
