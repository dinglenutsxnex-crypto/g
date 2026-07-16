using Godot;

[GlobalClass]
public partial class Generic6DOFJoint3D : Node3D
{
	public NodePath ExcludeNodes { get; set; }

	public void SetNodePath(NodePath path) { }

	public NodePath GetNodePath()
	{
		return null;
	}
}
