using System;
using System.Collections.Generic;
using Godot;
using Jint.Native;

public class JS : Node
{
	private static JS _instance;

	[Export]
	public bool enableLogging = true;

	public static JS Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: JS._Ready");
	}

	public object Evaluate(string script)
	{
		GD.Print("STUB: JS.Evaluate: " + script);
		return null;
	}

	public void CallFunction(string functionName, params object[] args)
	{
		GD.Print("STUB: JS.CallFunction: " + functionName);
	}

	public bool HasFunction(string functionName)
	{
		GD.Print("STUB: JS.HasFunction: " + functionName);
		return false;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
