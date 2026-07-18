using System;
using System.Collections.Generic;
using Godot;

public partial class GameDebugController : Node
{
	private static GameDebugController _instance;

	[Export]
	public bool isEnabled;

	[Export]
	public Label outputLabel;

	public static GameDebugController Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: GameDebugController._Ready");
	}

	public void Log(string message)
	{
		GD.Print("STUB [GAME_DEBUG]: " + message);
		if (outputLabel != null)
			outputLabel.Text += message + "\n";
	}

	public void Clear()
	{
		if (outputLabel != null)
			outputLabel.Text = string.Empty;
	}

	public void Toggle()
	{
		isEnabled = !isEnabled;
		Visible = isEnabled;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
