// ⚠️ STUB: needs full port — original used OnRenderImage/OnPreRender/OnPostRender, Graphics.Blit, Shader.EnableKeyword, RenderTexture camera work
using System;
using Godot;

[Tool]
public partial class ColorCorrectionEffect : Node3D
{
	[Export]
	public Material colorCorrectionMaterial;
	[Export]
	public float saturation = 1.2f;

	public ColorCorrectionEffect instance;
	public bool VignetteEnabled { get; set; }

	public void SetIntensity(float value) { }
	public void SetSaturation(float val) { }

	public override void _Ready()
	{
		instance = this;
	}

	public override void _ExitTree()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void EnableMatcap() { }
	public void DisableMatcap() { }
}
