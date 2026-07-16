using Godot;

public partial class BackgroundParallax : Node
{
	private Node3D _cameraTransform;

	private Vector3 _startCameraPosition;

	private Vector3 _startPosition;

	[Export]
	private float _parallaxKoef = 1f;

	[Export]
	private float _parallaxUVKoef = 1f;

	[Export]
	private float _lightmapParallaxUVKoef = 1f;

	private Material scrollMaterial;

	public override void _Ready()
	{
		_startPosition = GlobalPosition;
		_cameraTransform = GetViewport().GetCamera3D();
		_startCameraPosition = _cameraTransform.GlobalPosition;
		scrollMaterial = GetNode<MeshInstance3D>(".").GetSurfaceOverrideMaterial(0);
	}

	public override void _Process(double delta)
	{
		float num = (_startCameraPosition.X - _cameraTransform.GlobalPosition.X) * _parallaxKoef;
		if (scrollMaterial != null)
		{
			float num2 = _startCameraPosition.X - _cameraTransform.GlobalPosition.X;
			scrollMaterial.SetShaderParameter("_Offset", new Vector4(num2 * _parallaxUVKoef, 0f, 0f, 0f));
			scrollMaterial.SetShaderParameter("_LightmapOffset", new Vector4(num2 * _lightmapParallaxUVKoef, 0f, 0f, 0f));
		}
		num += _startPosition.X;
		Vector3 startPosition = _startPosition;
		startPosition.X = num;
		GlobalPosition = startPosition;
	}
}
