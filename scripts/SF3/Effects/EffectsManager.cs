using System;
using System.Collections.Generic;
using Godot;

namespace SF3.Effects
{
	public partial class EffectsManager : Node
	{
		private static EffectsManager _instance;

		[Export]
		private CameraMotionEffects _cameraMotion;

		[Export]
		private AnimationEffects _animationEffects;

		public static EffectsManager Instance
		{
			get { return _instance; }
		}

		public override void _Ready()
		{
			base._Ready();
			_instance = this;
		}

		public static void ShakeCamera(float frames, Vector3 amplitude, Vector3 period)
		{
			GD.Print("STUB: EffectsManager.ShakeCamera");
			if (_instance != null && _instance._cameraMotion != null)
			{
				_instance._cameraMotion.Shake(frames / 60f);
			}
		}

		public static void Reset()
		{
			GD.Print("STUB: EffectsManager.Reset");
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			if (_instance == this)
				_instance = null;
		}
	}
}
