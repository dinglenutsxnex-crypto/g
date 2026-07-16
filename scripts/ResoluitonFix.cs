using Godot;

public partial class ResoluitonFix : Node3D
{
	[Export]
	public float defAspectRatio = 1.5f;

	[Export]
	public bool y;

	[Export]
	public bool x;

	public override void _Ready()
	{
		Vector2 screenSize = DisplayServer.WindowGetSize();
		float aspect = (float)screenSize.X / (float)screenSize.Y;
		float ratio = aspect / defAspectRatio;
		Vector3 scale = Scale;
		if (y) scale.Y = ratio;
		if (x) scale.X = ratio;
		Scale = scale;
	}
}
