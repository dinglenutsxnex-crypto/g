// ⚠️ STUB: needs full port — original used SpriteRenderer, NGUI UISprite/UIWidget, BehaviourTimer
using System;
using System.Collections.Generic;
using Godot;

public partial class SpriteAlphaAnimation : Node2D
{
	public enum EAnimationStyle
	{
		LOOP,
		PING_PONG,
		ONCE
	}

	[Export]
	private float _animationTime = 8f;
	[Export]
	private Curve _animationCurve;
	[Export]
	private EAnimationStyle _animationStyle;
	[Export]
	private bool _activateOnStart = true;
	private float _timer;
	private float _alphaValue;
	[Export]
	private bool _useUnitySprites = true;
	private EAnimationStyle _currentAnimationStyle;
	private bool _backAnimation;
	private bool _active;

	public override void _Ready()
	{
		_timer = 0f;
		_backAnimation = false;
		if (_activateOnStart)
		{
			Activate(_animationTime);
		}
	}

	public void Activate(float timeSeconds)
	{
		Activate(_animationStyle, timeSeconds);
	}

	public void Activate(EAnimationStyle newAnimationStyle, float timeSeconds)
	{
		_currentAnimationStyle = newAnimationStyle;
		_timer = 0f;
		_active = true;
		_currentAnimationStyle = timeSeconds;
	}

	public override void _Process(double delta)
	{
		if (!_active)
		{
			return;
		}
		_timer += GameTimeController.deltaTimePaused;
		_alphaValue = _animationCurve.Sample((!_backAnimation) ? (_timer / _animationTime) : (1f - _timer / _animationTime));
		Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, _alphaValue);
		if (_timer >= _animationTime)
		{
			if (_currentAnimationStyle == EAnimationStyle.PING_PONG)
			{
				_backAnimation = !_backAnimation;
			}
			else if (_currentAnimationStyle == EAnimationStyle.LOOP)
			{
				_backAnimation = false;
			}
			else if (_currentAnimationStyle == EAnimationStyle.ONCE)
			{
				_backAnimation = false;
				_active = false;
			}
			_timer = 0f;
		}
	}
}
