using System;
using Godot;

namespace SF3.Effects
{
	public partial class CameraMotionEffects : Node
	{
		[Export]
		private Camera3D _targetCamera;

		[Export]
		private float _shakeIntensity = 0.5f;

		[Export]
		private float _shakeSpeed = 10f;

		private float _shakeTimer;

		private Vector3 _originalPosition;

		private bool _isShaking;

		public override void _Ready()
		{
			base._Ready();
			if (_targetCamera == null)
				_targetCamera = GetViewport().GetCamera3D();
			if (_targetCamera != null)
				_originalPosition = _targetCamera.Position;
		}

		public void Shake(float duration, float intensity = -1f)
		{
			_shakeTimer = duration;
			_isShaking = true;
			if (intensity > 0f)
				_shakeIntensity = intensity;
			if (_targetCamera != null)
				_originalPosition = _targetCamera.Position;
		}

		public void StopShake()
		{
			_isShaking = false;
			_shakeTimer = 0f;
			if (_targetCamera != null)
				_targetCamera.Position = _originalPosition;
		}

		public void DoDollyZoom(float targetFov, float duration)
		{
			if (_targetCamera != null)
			{
				Tween tween = CreateTween();
				tween.TweenProperty(_targetCamera, "fov", targetFov, duration);
			}
		}

		public override void _Process(double delta)
		{
			if (_isShaking && _shakeTimer > 0f)
			{
				_shakeTimer -= (float)delta;
				if (_targetCamera != null)
				{
					float offsetX = Mathf.Sin((float)Engine.GetProcessTime() * _shakeSpeed) * _shakeIntensity * 0.01f;
					float offsetY = Mathf.Cos((float)Engine.GetProcessTime() * _shakeSpeed * 1.3f) * _shakeIntensity * 0.01f;
					_targetCamera.Position = _originalPosition + new Vector3(offsetX, offsetY, 0f);
				}
				if (_shakeTimer <= 0f)
				{
					StopShake();
				}
			}
		}
	}
}
