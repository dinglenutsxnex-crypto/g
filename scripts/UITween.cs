using System;
using Godot;

public partial class UITween : Control
{
	public enum TweenTypes
	{
		Alpha,
		Position,
		Scale
	}

	public enum TweenBehaviourTypes
	{
		Once,
		Pinpong
	}

	public enum Directions
	{
		Forward,
		Backward
	}

	[Export]
	public TweenTypes Type;
	[Export]
	public TweenBehaviourTypes Behaviour;
	[Export]
	public Curve Curve;
	[Export]
	public Vector3 From;
	[Export]
	public Vector3 To;
	[Export]
	public Color clFrom;
	[Export]
	public Color clTo;
	[Export]
	public float Duration;
	[Export]
	public bool PlayOnAwake;

	private TextureRect _image;
	private float _time;
	private bool _isActive;
	private Directions _direction;
	private Action onFinished;

	public override void _Ready()
	{
		_image = GetNodeOrNull<TextureRect>(".");
		if (PlayOnAwake)
		{
			Play();
		}
	}

	public void Play()
	{
		Play(Behaviour);
	}

	public void Play(TweenBehaviourTypes type)
	{
		_time = 0f;
		_isActive = true;
		Behaviour = type;
	}

	public void Stop()
	{
		_isActive = false;
	}

	public override void _Process(double delta)
	{
		if (_isActive)
		{
			Go();
		}
	}

	private void Go()
	{
		if (_time < Duration)
		{
			_time += (float)Engine.GetProcessDeltaTime();
			Step((Math.Abs(_time) > 1E-05) ? (_time / Duration) : 0f);
			return;
		}
		if (Behaviour == TweenBehaviourTypes.Pinpong)
		{
			_time = 0f;
			_direction = (_direction == Directions.Forward) ? Directions.Backward : Directions.Forward;
			return;
		}
		_isActive = false;
		if (onFinished != null)
		{
			onFinished();
		}
	}

	private void Step(float position)
	{
		position = Mathf.Clamp(position, 0f, 1f);
		float t = Curve.Sample(position);
		switch (Type)
		{
		case TweenTypes.Alpha:
			switch (_direction)
			{
			case Directions.Forward:
				_image.Modulate = clFrom.Lerp(clTo, t);
				GD.PushWarning(_image.Modulate.ToString());
				break;
			case Directions.Backward:
				_image.Modulate = clTo.Lerp(clFrom, t);
				GD.PushWarning(_image.Modulate.ToString());
				break;
			}
			break;
		}
	}
}
