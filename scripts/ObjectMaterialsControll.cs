// ⚠️ STUB: needs full port — original used SkinnedMeshRenderer.sharedMesh.colors (vertex colors)
using Godot;

public partial class ObjectMaterialsControll : Node3D
{
	[Export]
	public Color changeTo = Colors.Red;
	[Export]
	public bool changeNow;

	public override void _Ready()
	{
		changeNow = false;
	}

	public override void _Process(double delta)
	{
	}
}
