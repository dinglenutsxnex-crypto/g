using Godot;

public partial class CustomSlideInput : Node
{
	public delegate void SlideInputEvent(Node target);

	private Camera3D _camera;

	public event SlideInputEvent onDown;

	public event SlideInputEvent onPress;

	public event SlideInputEvent onUp;

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>(".");
		if (_camera == null)
		{
			GD.PrintErr("Camera not found");
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			if (Input.IsMouseButtonPressed(MouseButton.Left) && !Input.IsMouseButtonPressed(MouseButton.Left))
			{
				CheckCollider(this.onDown);
			}
			if (Input.IsMouseButtonPressed(MouseButton.Left))
			{
				CheckCollider(this.onPress);
			}
		}
		else
		{
			CheckCollider(this.onUp);
		}
	}

	private void CheckCollider(SlideInputEvent action)
	{
		Vector2 mousePos = GetViewport().GetMousePosition();
		PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(_camera.ProjectRayOrigin(mousePos), _camera.ProjectRayOrigin(mousePos) + _camera.ProjectRayNormal(mousePos) * 1000f);
		var result = GetWorld3D().DirectSpaceState.IntersectRay(query);
		if (result.Count > 0 && action != null)
		{
			Node collider = (Node)result["collider"];
			if (collider != null)
			{
				action(collider);
			}
		}
	}
}
