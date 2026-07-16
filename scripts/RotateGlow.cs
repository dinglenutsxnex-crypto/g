using Godot;

public partial class RotateGlow : Node3D
{
	public Vector3 Angle;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		Quaternion q = Quaternion.FromEuler(Angle * SF3.GameTimeController.unscaledDeltaTime);
		Transform3D t = Transform;
		t.Basis = new Basis(t.Basis * q);
		Transform = t;
	}
}
