using Godot;

public partial class AlignObjectsInsideArea : Node3D
{
	[Export]
	public Node3D[] objectsToAlign;

	[Export]
	public float areaWidth;

	[Export]
	public float scaleFactor = 1f;

	public override void _Ready()
	{
		Vector2 screenSize = DisplayServer.WindowGetSize();
		float aspect = (float)screenSize.X / (float)screenSize.Y / 1.33f;
		Vector3 localPos = Position;
		float offset = scaleFactor * (areaWidth * aspect - areaWidth) / 2f;
		foreach (Node3D t in objectsToAlign)
		{
			localPos = t.Position;
			localPos.X += (t.Position.X * aspect - t.Position.X) * scaleFactor;
			localPos.X -= offset;
			t.Position = localPos;
		}
	}
}
