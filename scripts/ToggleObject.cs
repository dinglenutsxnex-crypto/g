using Godot;

public partial class ToggleObject : Node
{
	public override void _Ready()
	{
		Visible = false;
		Visible = true;
	}
}
