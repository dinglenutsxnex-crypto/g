using SF3.GameModels;
using Godot;

namespace SF3.Effects
{
	public abstract partial class GameEffectBase : Node3D, IGameEffect
	{
		protected Node3D transf;
		protected Node3D obj;
		protected Vector3 defaultAngles;
		protected Vector3 defaultScale;
		protected Vector3 defaultPos;

		public bool ignoreTimeScale;
		public Model model;

		public bool IsReady
		{
			get
			{
				return !Visible;
			}
		}

		public override void _Ready()
		{
			transf = this;
			obj = this;
			defaultPos = Position;
			defaultAngles = RotationDegrees;
			defaultScale = Scale;
			Disable();
			if (GetNodeOrNull<GpuParticles2D>(".") != null)
			{
				ParticleSystemCtrl particleSystemCtrl = GetNodeOrNull<ParticleSystemCtrl>(".");
				if (particleSystemCtrl == null)
				{
					particleSystemCtrl = new ParticleSystemCtrl();
					AddChild(particleSystemCtrl);
				}
				particleSystemCtrl.SetUnscaledUpdate(ignoreTimeScale);
			}
		}

		public void Create()
		{
		}

		public void Initialize()
		{
		}

		public void Play()
		{
		}

		public void Stop()
		{
		}

		public void StopForce()
		{
		}

		public void Reset()
		{
		}

		public virtual void Play(Model model)
		{
			this.model = model;
		}

		public virtual void Play(Model model, bool leftSide)
		{
		}

		public virtual void Play(Model model, bool leftSide, string alias, string[] values)
		{
		}

		public virtual void Play(Model model, bool leftSide, float yPos)
		{
		}

		public virtual void Play(Model model, bool leftSide, float yPos, string alias, string[] values)
		{
		}

		public virtual void Play(Model model, Vector3 atPos, Vector3 angles, int maxParticles, bool follow)
		{
			Play(model, atPos, angles, maxParticles);
		}

		public virtual void Play(Model model, Vector3 atPos, Vector3 angles, int maxParticles)
		{
			Play(model, atPos, angles);
		}

		public virtual void Play(Model model, Vector3 angle, bool applyToEnemy, string attachToBone, bool loop, bool follow, Vector3 shift)
		{
			Play(model, Vector3.Zero);
		}

		public virtual void Play(Model model, Vector3 angle, bool applyToEnemy, string attachToBone, bool loop, bool follow, Vector3 shift, Vector3 followAxis, bool attachLocal)
		{
			Play(model, Vector3.Zero);
		}

		public virtual void Play(Model model, Vector3 atPos, Vector3 angles, int maxParticles, string boneName, Vector3 offset, bool onEnemy, bool detached)
		{
			Play(model, atPos, angles);
		}

		public virtual void Play(Model model, Vector3 atPos, Vector3 angles)
		{
			Play(model, atPos);
			RotationDegrees = defaultAngles + angles;
		}

		public virtual void Play(Model model, Vector3 atPos, Quaternion _rotation)
		{
			Play(model, atPos);
			Quaternion = _rotation;
		}

		public virtual void Play(Model model, Vector3 atPos)
		{
			Position = atPos;
			this.model = model;
			Visible = true;
		}

		public virtual void Disable()
		{
			Visible = false;
		}

		public override void _Process(double delta)
		{
			if (!GameTimeController.systemPaused)
			{
				OnUpdate();
			}
		}

		protected virtual void OnUpdate()
		{
		}
	}
}
