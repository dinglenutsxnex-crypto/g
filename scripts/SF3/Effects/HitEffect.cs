using SF3.GameModels;
using Godot;

namespace SF3.Effects
{
	public partial class HitEffect : GameEffectBase
	{
		private GpuParticles2D particlesEffect;
		public float duration;
		public bool overrideDuration;
		private float _timer;
		private bool _checkOnDisable;
		private bool _follow;
		private Capsule _attachedTo;
		private Vector3 _offsetFromCapsule;
		private AnimationPlayer[] _animatorsComponents;

		public override void _Ready()
		{
			particlesEffect = GetNode<GpuParticles2D>(nameof(particlesEffect));
			if (!overrideDuration)
			{
				duration = GetDuration(particlesEffect);
			}
			_animatorsComponents = this.GetChildren().Where(c => c is AnimationPlayer).Cast<AnimationPlayer>().ToArray();
			base._Ready();
		}

		private float GetDuration(GpuParticles2D system)
		{
			return 1f;
		}

		public override void Play(Model m, Vector3 atPos, Vector3 angles, int maxParticles, bool follow)
		{
			SetMaxParticles(particlesEffect, maxParticles);
			base.Play(m, atPos, angles);
			particlesEffect.Restart();
			particlesEffect.Emitting = true;
			_checkOnDisable = true;
			_timer = 0f;
			_follow = follow;
			if (follow)
			{
				_attachedTo = Model.hitResult.StrikeData.collisionEdge;
				_offsetFromCapsule = _attachedTo.Center - atPos;
			}
			AnimationPlayer[] animatorsComponents = _animatorsComponents;
			foreach (AnimationPlayer animator in animatorsComponents)
			{
				animator.Play();
			}
		}

		private void SetMaxParticles(GpuParticles2D system, int count)
		{
			system.Amount = count;
		}

		public override void Disable()
		{
			AnimationPlayer[] animatorsComponents = _animatorsComponents;
			foreach (AnimationPlayer animator in animatorsComponents)
			{
				animator.Stop();
			}
			Visible = false;
		}

		protected override void OnUpdate()
		{
			if (_checkOnDisable)
			{
				if (_follow)
				{
					Position = _attachedTo.Center + _offsetFromCapsule;
				}
				_timer += GameTimeController.deltaTime;
				if (_timer >= duration)
				{
					_checkOnDisable = false;
					Disable();
				}
			}
		}
	}
}
