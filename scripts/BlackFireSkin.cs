// ⚠️ STUB: needs full port — original used SkinnedMeshRenderer bone remapping, coroutines, LateUpdate position calculations
using Godot;
using System.Collections.Generic;
using SF3.GameModels;

public partial class BlackFireSkin : Node3D
{
	public Node3D mainBone;
	public int upperMaterialRenderQueue;
	private Model model;
	public float baseAlpha = 2f;
	public Material[] materials;
	public Vector3 directionFactor;
	public Vector3 directionFacorMirror;
	public List<MeshInstance3D> skinnedMeshes;
	public float mirroringSpeed = 2f;
	public float directionChangeSpeed = 2f;

	public void Init(Node3D target, Node3D baseOrientation, Model model)
	{
		this.model = model;
	}

	public void SetColor(float t)
	{
	}

	public void _Process(double delta)
	{
	}
}
