using System;
using Godot;

public partial class DollyZoomEffect : Node
{
	[Export]
	private Camera3D _camera;

	[Export]
	private float _startFov = 60f;

	[Export]
	private float _endFov = 30f;

	[Export]
	private float _duration = 1.5f;

	[Export]
	private float _startDistance = 5f;

	[Export]
	private float _endDistance = 10f;

	private bool _isPlaying;

	private float _elapsed;

	private Vector3 _originalPosition;

	private float _originalFov;

	public override void _Ready()
	{
		base._Ready();
		if (_camera == null)
			_camera = GetViewport().GetCamera3D();
		if (_camera != null)
		{
			_originalPosition = _camera.Position;
			_originalFov = _camera.Fov;
		}
	}

	public void Play()
	{
		if (_camera == null)
			return;
		_isPlaying = true;
		_elapsed = 0f;
		Tween tween = CreateTween();
		tween.SetParallel(true);
		tween.TweenProperty(_camera, "fov", _endFov, _duration);
		tween.TweenProperty(_camera, "position", new Vector3(0f, 0f, _endDistance), _duration);
		tween.TweenCallback(Callable.From(() => _isPlaying = false));
	}

	public void PlayReverse()
	{
		if (_camera == null)
			return;
		_isPlaying = true;
		Tween tween = CreateTween();
		tween.SetParallel(true);
		tween.TweenProperty(_camera, "fov", _startFov, _duration);
		tween.TweenProperty(_camera, "position", _originalPosition, _duration);
		tween.TweenCallback(Callable.From(() => _isPlaying = false));
	}

	public void Stop()
	{
		_isPlaying = false;
	}

	public void Reset()
	{
		if (_camera != null)
		{
			_camera.Position = _originalPosition;
			_camera.Fov = _originalFov;
		}
		_isPlaying = false;
	}
}
