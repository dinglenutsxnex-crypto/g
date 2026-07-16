using Godot;

public partial class PreloaderUI : Node
{
	private static PreloaderUI instance;

	public static void ShowPreloader(bool show)
	{
		if (instance != null)
		{
			instance.ShowCanvas(show);
		}
	}

	public override void _Ready()
	{
		instance = this;
	}

	public void ShowCanvas(bool show)
	{
		Visible = show;
	}

	public override void _ExitTree()
	{
		instance = null;
	}
}
