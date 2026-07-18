using System;
using System.Collections.Generic;
using Godot;
using SF3;
using SF3.UserData;

public partial class QuestController : Node
{
	private static QuestController _instance;

	private List<object> _activeQuests = new List<object>();

	public static QuestController Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
	}

	public void StartQuest(string questId)
	{
		GD.Print("QuestController.StartQuest: " + questId);
	}

	public void CompleteQuest(string questId)
	{
		GD.Print("QuestController.CompleteQuest: " + questId);
	}

	public void FailQuest(string questId)
	{
		GD.Print("QuestController.FailQuest: " + questId);
	}

	public bool IsQuestActive(string questId)
	{
		return false;
	}

	public List<object> GetActiveQuests()
	{
		return _activeQuests;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
