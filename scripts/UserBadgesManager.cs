using System;
using System.Collections.Generic;
using Godot;

public partial class UserBadgesManager : Node
{
	private static UserBadgesManager _instance;

	[Export]
	public int maxBadges = 10;

	private List<string> _badges = new List<string>();

	public static UserBadgesManager Instance
	{
		get { return _instance; }
	}

	public enum BadgeTypes
	{
		Inventory,
		Perks,
		Boosters,
		Shop
	}

	public interface IBadgeItem
	{
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

	public void Reset(BadgeTypes badgeType, object data) { GD.Print("STUB: UserBadgesManager.Reset"); }
	public int GetNewPerksFor(object item) { GD.Print("STUB: UserBadgesManager.GetNewPerksFor"); return 0; }
	public void AddItem(BadgeTypes badgeType, object item) { GD.Print("STUB: UserBadgesManager.AddItem"); }
	public void RemoveItem(object item) { GD.Print("STUB: UserBadgesManager.RemoveItem"); }
	public void Clear(BadgeTypes badgeType) { GD.Print("STUB: UserBadgesManager.Clear"); }
	public void RegisterUnit(object unit) { }
	public void UnregisterUnit(object unit) { }
	public void UpdateItemsFor(object item) { }
	public List<object> WhichItemsisNew(BadgeTypes badgeType, object items) { return new List<object>(); }
	public int Count { get { return _badges.Count; } }

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
