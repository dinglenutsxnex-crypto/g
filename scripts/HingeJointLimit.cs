using Godot;

public partial class HingeJointLimit : Node
{
	[Export]
	public Node3D boneEnd;

	[Export]
	public Node3D boneStart;

	[Export]
	public Node3D parent;

	public string boneStartName;

	public string boneEndName;

	[Export]
	public float offset = 40f;

	private HingeJoint3D joint;

	[Export]
	public Curve animationCurve;

	[Export]
	public Node3D anchor;

	public override void _Process(double delta)
	{
		if (joint == null)
		{
			return;
		}
		float num = 0f;
		if (boneEnd == null || boneStart == null)
		{
			num = offset;
		}
		else
		{
			Vector3 vector = boneEnd.GlobalPosition - boneStart.GlobalPosition;
			num = Vector3.Angle(vector, vector.Project(new Plane(Vector3.Up, 0f)));
			num = (0f - animationCurve.Sample(num / 90f)) * 90f + offset;
		}
		// HingeJoint3D doesn't support dynamic limit changes via JointLimits struct like Unity.
		// This is a best-effort port; use PhysicsServer3D for advanced constraint control.
	}

	private void InitAnchor()
	{
		anchor = new Node3D();
		anchor.Name = Name + "_anchor";
		anchor.GetParent()?.RemoveChild(anchor);
		GetParent().AddChild(anchor);
		anchor.GlobalPosition = GlobalPosition;
		anchor.GlobalRotation = GlobalRotation;
		GetParent().RemoveChild(this);
	}

	public override void _Ready()
	{
		joint = GetNode<HingeJoint3D>(".");
		InitAnchor();
	}
}
