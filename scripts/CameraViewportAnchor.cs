using Godot;

public partial class CameraViewportAnchor : Node3D
{
	private const float DEFAULT_WIDTH = 1.33f;

	public float offsetByAspect;

	public float width = 500f;

	public float scaleFactor = 1f;

	public float temporaryFix = 1f;

	private Camera3D cam;

	public override void _Ready()
	{
		cam = GetNode<Camera3D>("..");
		if (cam != null)
		{
			Vector2 windowSize = DisplayServer.WindowGetSize();
			float aspect = (float)windowSize.X / (float)windowSize.Y / 1.33f;
			float offset = offsetByAspect * (aspect - 1f);
			Transform3D t = cam.Transform;
			t.Origin = new Vector3(t.Origin.X * aspect + offset, t.Origin.Y, t.Origin.Z);
			cam.Transform = t;
		}
	}
}
