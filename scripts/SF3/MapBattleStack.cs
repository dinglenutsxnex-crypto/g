using System;
using System.Collections.Generic;
using Godot;

public class MapBattleStack : Node
{
	private Stack<object> _battleStack = new Stack<object>();

	public int Count
	{
		get { return _battleStack.Count; }
	}

	public void Push(object battleData)
	{
		_battleStack.Push(battleData);
	}

	public object Pop()
	{
		if (_battleStack.Count > 0)
			return _battleStack.Pop();
		return null;
	}

	public object Peek()
	{
		if (_battleStack.Count > 0)
			return _battleStack.Peek();
		return null;
	}

	public void Clear()
	{
		_battleStack.Clear();
	}
}
