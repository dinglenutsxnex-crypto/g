using System;
using System.Collections.Generic;
using Godot;

public partial class BattleLog : Node
{
	private static BattleLog _instance;

	[Export]
	public Label logLabel;

	[Export]
	public ScrollContainer scrollContainer;

	private List<string> _entries = new List<string>();

	public static BattleLog Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: BattleLog._Ready");
	}

	public void Log(string message)
	{
		_entries.Add(message);
		GD.Print("STUB: BattleLog.Log: " + message);
		if (logLabel != null)
		{
			logLabel.Text += message + "\n";
		}
	}

	public void Clear()
	{
		_entries.Clear();
		if (logLabel != null)
			logLabel.Text = string.Empty;
	}

	public List<string> GetEntries()
	{
		return new List<string>(_entries);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
