using Godot;

public partial class OffsetFromLabelUI : Control
{
	[Export] public Label label;
	[Export] public float offsetX;

	private float savedWidth;

	public override void _Ready()
	{
		SetOffset();
	}

	private void SetOffset()
	{
		savedWidth = label.Size.X;
		Vector2 localPosition = label.Position;
		localPosition.X -= label.Size.X + offsetX;
		Position = localPosition;
	}

	public override void _Process(double delta)
	{
		if (savedWidth != label.Size.X)
		{
			SetOffset();
		}
	}
}