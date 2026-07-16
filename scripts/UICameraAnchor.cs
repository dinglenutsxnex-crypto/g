// ⚠️ STUB: needs full port — original used NGUI UICamera.onScreenResize, NGUITools.FindCameraForLayer, ScreenToWorldPoint
using Godot;

[Tool]
public partial class UICameraAnchor : Node3D
{
	public enum Side
	{
		BottomLeft, Left, TopLeft, Top, TopRight, Right, BottomRight, Bottom, Center
	}

	[Export]
	private Camera3D _uiCamera;
	[Export]
	private bool _anchorHorizontal = true;
	[Export]
	private bool _anchorVertical = true;
	[Export]
	private Side _side = Side.Center;
	[Export]
	private Vector3 _relativeOffset = Vector3.Zero;

	private Node3D _transform;
	private AnimationPlayer _animation;

	public override void _Ready()
	{
		_transform = this;
		_animation = GetNodeOrNull<AnimationPlayer>(".");
	}

	public void ForcedUpdate()
	{
	}
}
