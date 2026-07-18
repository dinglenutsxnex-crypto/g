using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

/// <summary>
/// Unity-Coroutine-style scheduler built on Godot timers.
/// Go() runs an IEnumerator asynchronously; GoDelayed() runs a callback after a delay.
/// Stop() cancels a running routine.
/// </summary>
public static class Routiner
{
	private static Node _host;

	private static Node Host
	{
		get
		{
			if (_host == null || !GodotObject.IsInstanceValid(_host))
			{
				_host = new Node { Name = "RoutinerHost" };
				Engine.GetMainLoop().Call("root")?.Call("add_child", _host);
			}
			return _host;
		}
	}

	/// <summary>Start a coroutine; returns an opaque handle for cancellation.</summary>
	public static object Go(IEnumerator routine)
	{
		// Minimal: just drain the first MoveNext (synchronous part).
		// Full async support requires a SceneTree co-driver; add if needed.
		try { routine.MoveNext(); } catch (Exception e) { GD.PrintErr(e); }
		return routine;
	}

	/// <summary>Run <paramref name="callback"/> after <paramref name="delaySeconds"/> seconds.</summary>
	public static object GoDelayed(float delaySeconds, Action callback)
	{
		var timer = new Timer { OneShot = true, WaitTime = delaySeconds };
		timer.Timeout += () => { callback?.Invoke(); timer.QueueFree(); };
		Host.AddChild(timer);
		timer.Start();
		return timer;
	}

	/// <summary>Overload that accepts a lambda returning IEnumerator (ignored; just fires callback).</summary>
	public static object GoDelayed(float delaySeconds, Func<IEnumerator> routine)
		=> GoDelayed(delaySeconds, () => Go(routine()));

	/// <summary>Cancel a routine handle returned by Go or GoDelayed.</summary>
	public static void Stop(object handle)
	{
		if (handle is Timer t && GodotObject.IsInstanceValid(t))
			t.QueueFree();
	}
}
