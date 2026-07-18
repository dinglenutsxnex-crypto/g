using System;
using System.Collections.Generic;
using Godot;
using SF3;
using SF3.UserData;

public partial class EnterPoint : Node
{
	[Export]
	private string _sceneName;

	private static EnterPoint _instance;

	private bool _initialized;

	public static EnterPoint Instance
	{
		get
		{
			return _instance;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
	}

	public void Initialize()
	{
		if (_initialized)
			return;
		_initialized = true;
		GD.Print("EnterPoint.Initialize: " + _sceneName);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
		{
			_instance = null;
		}
	}
}
