using Godot;

public partial class ActionButton : Button
{
	public delegate void ActionButtonPressed(bool isPressed);

	public event ActionButtonPressed Pressed;

	public override void _Ready()
	{
		ButtonDown += () => Pressed?.Invoke(true);
		ButtonUp += () => Pressed?.Invoke(false);
	}
}
