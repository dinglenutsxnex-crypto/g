using System;
using Godot;

public partial class GameRestarter : Node
{
	private static GameRestarter _instance;

	public static GameRestarter Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
	}

	public void Restart()
	{
		GD.Print("GameRestarter.Restart");
		GetTree().ReloadCurrentScene();
	}

	public void RestartWithDelay(float delay)
	{
		GD.Print("GameRestarter.RestartWithDelay: " + delay);
		GetTree().CreateTimer(delay).Timeout += Restart;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
