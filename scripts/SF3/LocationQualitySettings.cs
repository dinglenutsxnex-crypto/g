// ⚠️ STUB: needs full port — original used Unity Cloth, ParticleSystem, Graphics.Blit, RenderTexture
using System;
using Godot;

namespace SF3
{
	public partial class LocationQualitySettings : Node3D
	{
		[Serializable]
		public partial class ToggleQualLvl
		{
			public string quality;
			public bool enabled;
		}

		[Serializable]
		public partial class ParticlesQualLvl
		{
			public string quality;
			public float maxParticlesPercent;
		}

		[Serializable]
		public partial class ToggleObjSettingsLvls
		{
			public Node3D gameObject;
			public ToggleQualLvl[] settingsLevels;

			public void ApplyQuality()
			{
			}
		}

		[Serializable]
		public partial class ParticlesSettingsLvls
		{
			public Node3D particleSystem;
			public ParticlesQualLvl[] settingsLevels;

			public void Init()
			{
			}

			public void ApplyQuality()
			{
			}
		}

		public ToggleObjSettingsLvls[] gameObjectQualityLvls;
		public ParticlesSettingsLvls[] particlesQualityLvls;

		public override void _Ready()
		{
		}
	}
}
