using System;
using System.Collections.Generic;
using Godot;

public class TimerNode : IDisposable
{
	private static TimerNode _currentParent;

	private static string TAB = "     ";

	private static Dictionary<string, TimerNode> logs = new Dictionary<string, TimerNode>();

	private Timer _timer;

	private List<TimerNode> _children = new List<TimerNode>();

	public TimerNode(string tag, string parentTag = null)
	{
		_timer = new Timer(tag);
		if (!logs.ContainsKey(_timer.Tag))
		{
			logs.Add(_timer.Tag, this);
		}
		if (parentTag != null)
		{
			if (logs.ContainsKey(parentTag))
			{
				logs[parentTag].AddChild(this);
			}
			else if (_currentParent != null)
			{
				GD.PrintErr("No such parent: " + parentTag);
			}
		}
	}

	public static void SetParent(TimerNode t)
	{
		_currentParent = t;
	}

	public static void Clear()
	{
		_currentParent = null;
		logs.Clear();
	}

	private void Stop()
	{
		_timer.Stop();
	}

	private void AddChild(TimerNode node)
	{
		_children.Add(node);
	}

	public void Dispose()
	{
		Stop();
	}

	private void LogMe(int tabLevel = 0)
	{
		string text = TAB;
		for (int i = 0; i < tabLevel; i++)
		{
			text += TAB;
		}
		GD.Print(text + _timer.ToString());
	}

	private void LogMeRecursive(int tabLevel = 0)
	{
		LogMe(tabLevel);
		foreach (TimerNode child in _children)
		{
			child.LogMeRecursive(tabLevel + 1);
		}
	}

	public static void LogHierarchy()
	{
		if (!_currentParent._timer.Stopped)
		{
			_currentParent.Stop();
		}
		GD.Print("================================  LOADING TIME RESULTS:  ================================");
		_currentParent.LogMeRecursive();
		GD.Print("========================================= END  =========================================");
	}
}
