using System;
using Godot;

public class SceneInitializer : Node
{
	[Export]
	private string _sceneType;

	private bool _initialized;

	public override void _Ready()
	{
		base._Ready();
		Initialize();
	}

	public void Initialize()
	{
		if (_initialized)
			return;
		_initialized = true;
		GD.Print("SceneInitializer.Initialize: " + _sceneType);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}
}
