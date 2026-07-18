using System;
using Godot;

public partial class GameLoad : Node
{
	private static GameLoad _instance;

	[Export]
	public Label statusLabel;

	[Export]
	public ProgressBar progressBar;

	public static GameLoad Instance
	{
		get { return _instance; }
	}

	public override void _Ready()
	{
		base._Ready();
		_instance = this;
		GD.Print("STUB: GameLoad._Ready");
	}

	public void SetStatus(string status)
	{
		GD.Print("STUB: GameLoad.SetStatus: " + status);
		if (statusLabel != null)
			statusLabel.Text = status;
	}

	public void SetProgress(float progress)
	{
		if (progressBar != null)
			progressBar.Value = progress * 100.0;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_instance == this)
			_instance = null;
	}
}
