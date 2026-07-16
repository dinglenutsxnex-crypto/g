using System;
using Godot;

public class SF3FPSLogger : Node
{
	private static SF3FPSLogger _instance;

	[Export]
	private float _updateInterval = 1.0f;

	private float _elapsed;
	private int _frameCount;
	private float _fps;

	public static SF3FPSLogger Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
	}

	public float GetFPS()
	{
		return _fps;
	}

	public override void _Process(double delta)
	{
		_elapsed += (float)delta;
		_frameCount++;
		if (_elapsed >= _updateInterval)
		{
			_fps = _frameCount / _elapsed;
			_frameCount = 0;
			_elapsed = 0f;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
