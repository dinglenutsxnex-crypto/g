using System;
using Godot;

public partial class SSceneManager : Node
{
	private static SSceneManager _instance;

	[Export]
	public string currentScene;

	public static SSceneManager Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: SceneManager._Ready");
	}

	public void LoadScene(string scenePath)
	{
		GD.Print("STUB: SceneManager.LoadScene: " + scenePath);
	}

	public void ReloadCurrentScene()
	{
		GD.Print("STUB: SceneManager.ReloadCurrentScene");
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
