using System;
using System.Collections.Generic;
using Godot;
using SF3;

public class MissionsHolder : Node
{
	private List<object> _missions = new List<object>();

	public int Count
	{
		get { return _missions.Count; }
	}

	public void AddMission(object mission)
	{
		_missions.Add(mission);
	}

	public void RemoveMission(object mission)
	{
		_missions.Remove(mission);
	}

	public object GetMission(int index)
	{
		if (index >= 0 && index < _missions.Count)
			return _missions[index];
		return null;
	}

	public void Clear()
	{
		_missions.Clear();
	}

	public void RefreshMissions()
	{
		GD.Print("MissionsHolder.RefreshMissions");
	}
}
