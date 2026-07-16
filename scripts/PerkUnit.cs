// ⚠️ STUB: needs full port — original used NGUI UISprite, UITweener, BehaviourTimer, ColorUtility
using System;
using SF3.Utils;
using Godot;

public partial class PerkUnit : Control
{
	[Export]
	private TextureRect progress;
	[Export]
	private TextureRect background;
	[Export]
	private TextureRect whiteBack;
	private int _index;
	private float _size;
	private Vector2 _targetPosition;
	[Export]
	private float _delimeterSize;
	[Export]
	private float _speed;

	public void Init(string color, string spriteName, int duration, Action<object> onDone, int index, float size, bool infinite, bool showExpiration, string actionName)
	{
		UpdateSize(size);
		UpdateIndex(index);
		Name = actionName;
	}

	public void UpdateIndex(int index)
	{
		_index = index;
		UpdateTargetPosition(false);
	}

	public void UpdateSize(float size)
	{
		_size = size;
		UpdateTargetPosition(true);
	}

	private void UpdateTargetPosition(bool instant)
	{
		_targetPosition = new Vector2((_size + _size * (_delimeterSize / 100f)) * (float)_index, 0f);
		if (instant)
		{
			Position = _targetPosition;
		}
	}

	public override void _Process(double delta)
	{
		if (Position.DistanceTo(_targetPosition) > 0.01f)
		{
			Position = Position.Lerp(_targetPosition, (float)delta * _speed);
		}
	}

	public void AnimateShow()
	{
	}

	public void AnimateHide(Action onDone)
	{
		onDone();
		QueueFree();
	}
}
