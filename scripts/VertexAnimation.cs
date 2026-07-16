using Godot;

public partial class VertexAnimation : Node3D
{
	[Export]
	private Curve vertexMovementCurve;

	[Export]
	private float factorX;

	[Export]
	private float factorY;

	[Export]
	private float animCycleTime = 1f;

	private Material material;

	private bool moveForward;

	private float timer;

	private const string MATERIAL_PARAMETER_NAME_1 = "_VertexAnimationFactorX";

	private const string MATERIAL_PARAMETER_NAME_2 = "_VertexAnimationFactorY";

	private MeshInstance3D meshRender;

	public override void _Ready()
	{
		meshRender = GetNode<MeshInstance3D>(".");
		material = meshRender.MaterialOverride;
	}

	public override void _Process(double delta)
	{
		if (meshRender.MaterialOverride != material)
		{
			material = meshRender.MaterialOverride;
		}
		timer += SF3.GameTimeController.deltaTimePaused;
		float num = timer / animCycleTime;
		if (!moveForward)
		{
			num = 1f - num;
		}
		material.SetShaderParameter("_VertexAnimationFactorX", vertexMovementCurve.Sample(num) * factorX);
		material.SetShaderParameter("_VertexAnimationFactorY", vertexMovementCurve.Sample(num) * factorY);
		if (timer >= animCycleTime)
		{
			timer = 0f;
			moveForward = !moveForward;
		}
	}
}
