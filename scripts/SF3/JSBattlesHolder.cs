using System;
using System.Collections.Generic;
using Godot;
using SF3.UserData;
using sf3DTO;

public class JSBattlesHolder : Node
{
	private List<object> _battles = new List<object>();

	public int Count
	{
		get { return _battles.Count; }
	}

	public void AddBattle(object battleData)
	{
		_battles.Add(battleData);
	}

	public void RemoveBattle(object battleData)
	{
		_battles.Remove(battleData);
	}

	public object GetBattle(int index)
	{
		if (index >= 0 && index < _battles.Count)
			return _battles[index];
		return null;
	}

	public void Clear()
	{
		_battles.Clear();
	}

	public void RefreshBattles()
	{
		GD.Print("JSBattlesHolder.RefreshBattles");
	}
}
