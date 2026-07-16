using Godot;

public partial class PickDepthFromParent : Control
{
	[Export]
	public Control parent;

	[Export]
	public int depthOffset;

	public override void _Ready()
	{
	}

	public void ManualUpdate()
	{
		if (parent != null && ZIndex != parent.ZIndex + depthOffset)
		{
			ZIndex = parent.ZIndex + depthOffset;
		}
	}

	public override void _Process(double delta)
	{
		ManualUpdate();
	}
}
