using Godot;

public partial class HorizontalScaleForResolution : Node
{
	private const float DEFAULT_ASPECT = 1.33f;

	[Export]
	public float scaleFactor = 1f;

	public override void _Ready()
	{
		Vector2I screenSize = DisplayServer.WindowGetSize();
		float num = (float)screenSize.X / (float)screenSize.Y / 1.33f;
		Control component = GetNode<Control>(".");
		Vector3 position = Position;
		position.X = 0f;
		Position = position;
		if (component != null)
		{
			component.Size = new Vector2(component.Size.X + Mathf.RoundToInt(scaleFactor * (component.Size.X * num - component.Size.X)), component.Size.Y);
		}
	}
}
