// ⚠️ STUB: needs full port — original used OnDrawGizmos (editor-only visualization)
using Godot;

[Tool]
public partial class DrawCircle : Node3D
{
	[Export]
	public float radius;
	[Export]
	public float angle;
	[Export]
	public float steps;
}
