using Godot;

public class ScreenLoader : Node
{
	public override void _Ready()
	{
		GetTree().ChangeSceneToFile("res://scenes/loadScreen.tscn");
	}
}
