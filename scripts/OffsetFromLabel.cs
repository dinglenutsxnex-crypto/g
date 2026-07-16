using Godot;

public partial class OffsetFromLabel : Node
{
	private Label label;
	private float offsetX;
	private float offsetY;
	private int savedWidth;
	private bool alignToStartPos;
	private float startXPos;

	public override void _Ready()
	{
		label = GetNode<Label>(".");
		if (label != null)
		{
			savedWidth = (int)label.Size.X;
			SetOffset();
			if (alignToStartPos)
			{
				startXPos *= (float)DisplayServer.WindowGetSize().X / (float)DisplayServer.WindowGetSize().Y / 1.33f;
				AlignToStartPos();
			}
		}
	}

	private void SetOffset()
	{
		Vector2 pos = Position;
		pos.X -= label.Size.X + offsetX;
		pos.Y += offsetY;
		Position = pos;
	}

	private void AlignToStartPos()
	{
		float num = label.Position.X - Position.X;
		Vector2 labelPos = label.Position;
		labelPos.X = startXPos + num / 2f;
		label.Position = labelPos;
		SetOffset();
	}

	public override void _Process(double delta)
	{
		if (savedWidth != (int)label.Size.X)
		{
			SetOffset();
			AlignToStartPos();
		}
	}
}
