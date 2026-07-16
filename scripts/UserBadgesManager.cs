using System;
using System.Collections.Generic;
using Godot;

public class UserBadgesManager : Node
{
	private static UserBadgesManager _instance;

	[Export]
	public int maxBadges = 10;

	private List<string> _badges = new List<string>();

	public static UserBadgesManager Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: UserBadgesManager._Ready");
	}

	public void AddBadge(string badgeId)
	{
		if (_badges.Count < maxBadges)
		{
			_badges.Add(badgeId);
			GD.Print("STUB: UserBadgesManager.AddBadge: " + badgeId);
		}
	}

	public void RemoveBadge(string badgeId)
	{
		_badges.Remove(badgeId);
		GD.Print("STUB: UserBadgesManager.RemoveBadge: " + badgeId);
	}

	public bool HasBadge(string badgeId)
	{
		return _badges.Contains(badgeId);
	}

	public List<string> GetBadges()
	{
		return new List<string>(_badges);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
