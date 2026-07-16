// ⚠️ STUB: needs full port — original used Camera, RenderTexture, Shader, Graphics.Blit, Camera.RenderWithShader
using Godot;

public partial class FireEffect : Node3D
{
	[Export]
	private int factor = 4;
	[Export]
	private float erosionSpread = 2f;
	[Export]
	private ShaderMaterial fireBoundsShader;
	[Export]
	private ShaderMaterial outputMat;
	[Export]
	private ShaderMaterial erosionShader;

	private Camera3D origCamera;
	private Camera3D shaderCamera;

	public override void _Ready()
	{
	}

	public void OnEffectEnable()
	{
	}

	public override void _ExitTree()
	{
	}
}
