using Godot;

public partial class ShaderInfo : Node3D
{
	[Export]
	public ShaderMaterial ShaderMat;

	[Export]
	public MeshInstance3D Renderer;

	public override void _Ready()
	{
		if (Renderer != null && ShaderMat != null)
		{
			Renderer.MaterialOverride = ShaderMat;
		}
	}
}
