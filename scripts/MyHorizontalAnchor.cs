using Godot;

public partial class MyHorizontalAnchor : Node3D
{
	public enum AnchorSide
	{
		left = 0,
		right = 1,
		center = 2
	}

	public AnchorSide anchor;

	public float worldUnitsOffset;

	public Camera3D uiCam;

	private bool searchingForCamera;

	public bool useAspectRatio;

	public float scaleByAspect;

	public override void _Ready()
	{
		searchingForCamera = true;
		SetPosition();
	}

	public override void _EnterTree()
	{
		SetPosition();
	}

	private void SetPosition()
	{
		Viewport viewport = GetViewport();
		if (viewport != null)
		{
			Camera3D cam = viewport.GetCamera3D();
			if (cam != null)
			{
				uiCam = cam;
				searchingForCamera = false;
				Vector3 camPos = cam.GlobalPosition;
				Vector2 vpPos = cam.UnprojectPosition(GlobalPosition);
				float targetX = vpPos.X;
				if (anchor == AnchorSide.left)
				{
					targetX = 0f;
				}
				else if (anchor == AnchorSide.center)
				{
					targetX = 0.5f;
				}
				Vector2 windowSize = DisplayServer.WindowGetSize();
				float offsetX = worldUnitsOffset;
				if (useAspectRatio)
				{
					float aspect = (float)windowSize.X / (float)windowSize.Y / 1.33f - 1f;
					offsetX = worldUnitsOffset + scaleByAspect * aspect;
				}
				Vector3 worldTarget = cam.ProjectPosition(new Vector2(targetX * windowSize.X, vpPos.Y * windowSize.Y), 0f);
				GlobalPosition = new Vector3(worldTarget.X + offsetX, GlobalPosition.Y, GlobalPosition.Z);
			}
		}
	}

	public override void _Process(double delta)
	{
		if (searchingForCamera)
		{
			SetPosition();
		}
	}
}
