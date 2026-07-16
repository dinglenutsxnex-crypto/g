using Godot;

namespace SF3.Effects
{
	public partial class ParticleSystemCtrl : Node
	{
		private bool _unscaledUpdate;

		private GpuParticles2D[] _particles;

		public override void _Ready()
		{
			_particles = GetChildren().ConvertTo<GpuParticles2D[]>();
		}

		public override void _Process(double delta)
		{
			if (_unscaledUpdate && !GameTimeController.systemPaused)
			{
				if (GameTimeController.timeScale < 0.01f)
				{
					if (_particles.Length > 0)
						_particles[0].Restart();
				}
				else
				{
					UnPauseParticles();
				}
			}
		}

		private void UnPauseParticles()
		{
			if (_particles != null && _particles.Length > 0)
			{
				_particles[0].Emitting = true;
			}
		}

		public void SetUnscaledUpdate(bool enableUnscaledUpdate)
		{
			_unscaledUpdate = enableUnscaledUpdate;
			if (!_unscaledUpdate)
			{
				UnPauseParticles();
			}
		}
	}
}
