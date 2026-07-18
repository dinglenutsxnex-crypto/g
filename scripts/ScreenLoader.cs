using Godot;

public partial class ScreenLoader : Node
{
	public override void _Ready()
	{
		GetTree().ChangeSceneToFile("res://scenes/loadScreen.tscn");
	}
}
