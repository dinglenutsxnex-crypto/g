using Godot;
using System;
using SF3;

public class StickCenter : ExtentionBehaviour
{
	private Camera3D _uiCamera;
	private Vector3 TargetPosition;
	private Vector3 BasePosition;
	private Vector3 CaclulatadPosision;

	public float MaxRadius;
	public float SafeRadius;
	private bool _inDrag;
	public float DeltaSpeed = 1.2f;
	private Stick _joystick;
	private MultiTweenTransition _multiTween;
	private SphereShape3D SphereCollider;

	public uint JoystickLayer;

	private bool InSafeRadius
	{
		get
		{
			return GlobalPosition.DistanceTo(BasePosition) < SafeRadius;
		}
	}

	private Vector3 baseDeltaTouch
	{
		get
		{
			if (SystemProperties.IsMobilePlatform)
			{
				for (int i = 0; i < Input.GetConnectedJoypads().Count; i++)
				{
					// Simplified mobile touch handling
				}
				return Vector3.Zero;
			}
			// Desktop mouse handling
			return Vector3.Zero;
		}
	}

	public override void _Ready()
	{
		BasePosition = Position;
		TargetPosition = BasePosition;
		CaclulatadPosision = BasePosition;
		_multiTween.TweenOut();
		_uiCamera = GetViewport().GetCamera3D();
		CollisionLayer = 30;
	}

	public override void _Process(double delta)
	{
		Position = Position.ClampLength(MaxRadius);
		if (_inDrag)
		{
			_joystick.DragProcess(this, InSafeRadius);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			OnDragStart();
		}
		else
		{
			OnDragEnd();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		Vector3 vector = baseDeltaTouch;
		TargetPosition = ((!(vector == Vector3.Zero)) ? vector : TargetPosition);
	}

	private void OnDragStart()
	{
		SphereCollider.Radius = 300f;
		TargetPosition = CaclulatadPosision + baseDeltaTouch;
		_inDrag = true;
		_multiTween.TweenIn();
		OnDrag(Vector2.Zero);
	}

	private void OnDragEnd()
	{
		_multiTween.TweenOut();
		_joystick.Release();
		TargetPosition = BasePosition;
		_inDrag = false;
		SphereCollider.Radius = 200f;
	}
}
