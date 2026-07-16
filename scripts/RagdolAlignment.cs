using Godot;

public partial class RagdolAlignment : Node
{
	[Export]
	public float boundsWidth = 10f;

	[Export]
	public float maxAngle = 10f;

	public Node3D transf;

	public RigidBody3D rigidBody;

	private Vector3 _pos;

	private Vector3 _startPos;

	private bool _inPhysics;

	private float _startAngle;

	public void Initialize(Node3D rootBone)
	{
		transf = rootBone;
		rigidBody = transf.GetNode<RigidBody3D>(".");
	}

	public void Activate(bool zImpulse)
	{
		_startPos = transf.GlobalPosition;
		_inPhysics = true;
	}

	private void ClampAngle(float baseAngle)
	{
		_pos = transf.Rotation;
		float num = baseAngle - _pos.Y;
		if (Mathf.Abs(num) > maxAngle)
		{
			_pos.Y = baseAngle + maxAngle * Mathf.Sign(0f - num);
		}
		transf.Rotation = _pos;
	}

	public override void _Process(double delta)
	{
		if (_inPhysics)
		{
			_pos = transf.GlobalPosition;
			_pos.Z = Mathf.Clamp(_pos.Z, _startPos.Z - boundsWidth, _startPos.Z + boundsWidth);
			transf.GlobalPosition = _pos;
			_inPhysics = !rigidBody.Freeze;
		}
	}
}
