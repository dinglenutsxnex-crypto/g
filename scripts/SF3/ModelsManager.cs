using System;
using System.Collections.Generic;
using Godot;

public partial class ModelsManager : Node
{
	private static ModelsManager _instance;

	[Export]
	public Node Player;

	[Export]
	public Node Enemy;

	public static ModelsManager Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: ModelsManager._Ready");
	}

	public List<object> GetAllRules()
	{
		GD.Print("STUB: ModelsManager.GetAllRules");
		return new List<object>();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
