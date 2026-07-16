using System;
using System.Collections.Generic;
using Godot;

public partial class SpriteSlicerSFManager : Node2D
{
	private List<SpriteSlicer2DSliceInfo> _slicedSpriteInfo = new List<SpriteSlicer2DSliceInfo>();

	private TrailRenderer2D _trailRenderer;

	private const float MAX_ANGLE_DEVIATION_WITHIN_SWIPE = 5f;

	private const float MOUSE_RECORD_INTERVAL = 0.05f;

	private const int MAX_MOUSE_POSITIONS = 5;

	private List<Vector2> _mousePositions = new List<Vector2>();

	private float _mouseRecordTimer;

	private Vector2 _swipeStart = Vector2.Zero;

	private Vector2 _swipeEnd = Vector2.Zero;

	[Export]
	public float cutForceMultiplier;

	[Export]
	public float slowingFactor;

	[Export]
	public float swipeLengthMultiplier;

	[Export]
	public GpuParticles2D sliceParticles;

	public Node2D slashParent;

	public float explodeTorqueForce = 1f;

	public PackedScene slashPrf;

	public Action onSliced;

	private List<RigidBody2D> _childSpriteRigidbodies;

	private List<BoosterpackSlash> _slashs;

	public Material SharedMaterial
	{
		get
		{
			if (_slicedSpriteInfo.Count > 0 && _slicedSpriteInfo[0].ChildObjects.Count > 0)
			{
				Sprite2D sprite = _slicedSpriteInfo[0].ChildObjects[0] as Sprite2D;
				if (sprite != null)
				{
					return sprite.Material;
				}
			}
			return null;
		}
	}

	public override void _Ready()
	{
		_trailRenderer = GetNode<TrailRenderer2D>("TrailRenderer2D");
		EnableTrailParticles(false);
		_childSpriteRigidbodies = new List<RigidBody2D>();
		_slashs = new List<BoosterpackSlash>();
	}

	public override void _Process(double delta)
	{
		UpdateVelocity();
	}

	public override void _EnterTree()
	{
		EnableTrailParticles(false);
	}

	public void OnDragStart()
	{
		RecordMousePosition();
		UpdateTrail();
	}

	public void OnDrag(Vector2 delta)
	{
		RecordMousePosition();
		UpdateTrail();
		if (TryEndSwipe())
		{
			SwipeEnd();
		}
	}

	public void OnDragEnd()
	{
		EnableTrailParticles(false);
		if (_swipeEnd == Vector2.Zero && _mousePositions.Count > 0)
		{
			_swipeEnd = _mousePositions[_mousePositions.Count - 1];
		}
		SwipeEnd();
	}

	private void SwipeEnd()
	{
		SliceIntersectedSprites();
		ClearPositions();
		FixChildrensLayer();
		_slicedSpriteInfo.Clear();
	}

	private bool RecordMousePosition()
	{
		_mouseRecordTimer -= (float)Engine.GetProcessDeltaTime();
		if (_mouseRecordTimer <= 0f)
		{
			Vector2 mousePos = GetGlobalMousePosition();
			_mousePositions.Add(mousePos);
			_mouseRecordTimer = 0.05f;
			if (_swipeStart == Vector2.Zero)
			{
				_swipeStart = mousePos;
			}
			if (_mousePositions.Count > 5)
			{
				_mousePositions.RemoveAt(0);
			}
			return true;
		}
		return false;
	}

	private bool TryEndSwipe()
	{
		if (_mousePositions.Count < 2 || _swipeStart == Vector2.Zero)
		{
			return false;
		}
		Vector2 last = _mousePositions[_mousePositions.Count - 1];
		Vector2 from = last - _mousePositions[_mousePositions.Count - 2];
		Vector2 to = last - _swipeStart;
		float angle = Mathf.RadToDeg(from.AngleTo(to));
		if (Math.Abs(angle) < 5f)
		{
			return false;
		}
		_swipeEnd = last;
		return true;
	}

	public void Explode(Vector2 center, float radius, float force)
	{
		for (int i = 0; i < _slashs.Count; i++)
		{
			_slashs[i].PlayBeforeExplosionAnimation();
		}
		for (int j = 0; j < _childSpriteRigidbodies.Count; j++)
		{
			RigidBody2D rb = _childSpriteRigidbodies[j];
			if (rb != null)
			{
				Vector2 diff = rb.GlobalPosition - center;
				float magnitude = diff.Length();
				float factor = 1f - magnitude / radius;
				rb.ApplyImpulse(diff.Normalized() * force * factor);
				rb.Freeze = false;
				float dir = (diff.X < 0f) ? 1f : (-1f);
				rb.ApplyTorqueImpulse(explodeTorqueForce * dir);
			}
		}
	}

	public void ClearPositions()
	{
		_mousePositions.Clear();
		_swipeStart = Vector2.Zero;
		_swipeEnd = Vector2.Zero;
	}

	public void SimulateSlicing(Vector2 worldPositionStart, Vector2 worldPositionEnd)
	{
		ClearPositions();
		_mousePositions.Add(worldPositionStart);
		UpdateTrail();
		_mousePositions.Add(worldPositionEnd);
		UpdateTrail();
		_swipeStart = worldPositionStart;
		_swipeEnd = worldPositionEnd;
		SliceIntersectedSprites();
		ClearPositions();
		_slicedSpriteInfo.Clear();
	}

	private bool SliceIntersectedSprites()
	{
		if (_swipeEnd == Vector2.Zero || !IsSwipeHitsSlicableSprite(_swipeStart, _swipeEnd))
		{
			return false;
		}
		Vector2 vector = _swipeEnd - _swipeStart;
		Vector2 start = _swipeStart + vector * (0f - (swipeLengthMultiplier - 1f));
		Vector2 end = _swipeEnd + vector * (swipeLengthMultiplier - 1f);
		vector = vector.Normalized();
		SpriteSlicer2D.SliceAllSprites(start, end, true, ref _slicedSpriteInfo);
		Vector2 perp = vector;
		_childSpriteRigidbodies.RemoveAll((RigidBody2D rb) => rb == null);
		if (_slicedSpriteInfo.Count > 0)
		{
			for (int i = 0; i < _slicedSpriteInfo.Count; i++)
			{
				perp = new Vector2(vector.Y, -vector.X);
				Vector2 sliceEnter = _slicedSpriteInfo[i].SliceEnterWorldPosition;
				Vector2 sliceExit = _slicedSpriteInfo[i].SliceExitWorldPosition;
				Vector2 pos = (sliceExit - sliceEnter) / 2f + sliceEnter;
				for (int j = 0; j < _slicedSpriteInfo[i].ChildObjects.Count; j++)
				{
					Node2D child = _slicedSpriteInfo[i].ChildObjects[j] as Node2D;
					if (child == null) continue;
					RigidBody2D rb = child as RigidBody2D;
					if (rb == null) continue;
					perp = -perp;
					rb.ApplyImpulse(perp * cutForceMultiplier * rb.Mass, pos);
					_childSpriteRigidbodies.Add(rb);
				}
			}
			BoosterpackSlash slash = CreateSlash(start, end, _slicedSpriteInfo[0].SliceEnterWorldPosition, _slicedSpriteInfo[_slicedSpriteInfo.Count - 1].SliceExitWorldPosition);
			slash.PlaySwipeTrailAnimation();
			_slashs.Add(slash);
			if (onSliced != null)
			{
				onSliced();
			}
			return true;
		}
		return false;
	}

	public void EnableTrailParticles(bool isEnabled = true)
	{
		if (sliceParticles == null) return;
		if (isEnabled)
		{
			if (!sliceParticles.Emitting)
			{
				sliceParticles.Emitting = true;
			}
		}
		else if (sliceParticles.Emitting)
		{
			sliceParticles.Emitting = false;
		}
	}

	private bool IsSwipeHitsSlicableSprite(Vector2 cutStart, Vector2 cutEnd)
	{
		Vector2 dir = (cutEnd - cutStart).Normalized();
		float distance = cutEnd.DistanceTo(cutStart);
		PhysicsRayQueryParameters2D query = PhysicsRayQueryParameters2D.Create(cutStart, cutStart + dir * distance);
		Godot.Collections.Dictionary result = GetWorld2D().DirectSpaceState.IntersectRay(query);
		return result.Count > 0;
	}

	private void ResetTrail()
	{
		if (_trailRenderer != null)
		{
			_trailRenderer.Clear();
		}
	}

	private void UpdateTrail()
	{
		if (_trailRenderer != null && _mousePositions.Count > 0)
		{
			Vector2 lastPos = _mousePositions[_mousePositions.Count - 1];
			_trailRenderer.GlobalPosition = new Vector2(lastPos.X - 0.5f, lastPos.Y);
			EnableTrailParticles();
		}
	}

	private void UpdateVelocity()
	{
		for (int i = 0; i < _childSpriteRigidbodies.Count; i++)
		{
			RigidBody2D rb = _childSpriteRigidbodies[i];
			if (rb != null && rb.LinearVelocity.Length() > 0f)
			{
				rb.LinearVelocity = rb.LinearVelocity * slowingFactor;
			}
		}
	}

	private void FixChildrensLayer()
	{
		for (int i = 0; i < _slicedSpriteInfo.Count; i++)
		{
			for (int j = 0; j < _slicedSpriteInfo[i].ChildObjects.Count; j++)
			{
				Node2D child = _slicedSpriteInfo[i].ChildObjects[j] as Node2D;
				if (child != null)
				{
					Vector2 localPos = child.Position;
					localPos.Y = -1f;
					child.Position = localPos;
				}
			}
		}
	}

	private BoosterpackSlash CreateSlash(Vector2 start, Vector2 end, Vector2 cutEnter, Vector2 cutOut)
	{
		Node2D instance = slashPrf.Instantiate<Node2D>();
		slashParent.AddChild(instance);
		Vector2 v = end - start;
		Vector2 to = v.Normalized();
		float angle = Mathf.RadToDeg(Vector2.Right.AngleTo(to));
		if (end.Y < start.Y)
		{
			angle *= -1f;
		}
		instance.Rotation = Mathf.DegToRad(angle);
		instance.Position = new Vector2(start.X + v.X / 2f, start.Y + v.Y / 2f);
		BoosterpackSlash component = instance as BoosterpackSlash;
		component.SetFierPositionAndSize(cutEnter, cutOut);
		return component;
	}
}
