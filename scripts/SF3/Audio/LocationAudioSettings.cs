using System;
using Godot;

namespace SF3.Audio
{
	public partial class LocationAudioSettings : Node
	{
		[Serializable]
		public partial class ParamsEQ
		{
			public float centerFreq = 8000f;
			public float octaveRange = 1f;
			public float frequencyGain = 1f;
		}

		public AudioSourceSettings audioSettings;
		public AudioStream shadowFormAmbient;
		public ParamsEQ defaultEQParams;
		public ParamsEQ shadowFormEQParams;
		public Curve shadowFormEQCurve;
		public float eqAnimationTime;
		public AudioStreamPlayer3D _shadowSource;
		public AudioStreamPlayer3D _musicSource;
		private float _timer;
		private bool _activateShadow;
		private bool _isActivateShadow;
		private static string _musicName;

		public static LocationAudioSettings Instance { get; private set; }
		public static bool NeedSpecialShadowFormSound { get; set; }

		public static void SetPitch(float pitchValue)
		{
			Instance._musicSource.PitchScale = pitchValue;
		}

		public static void SetVolume(float volume)
		{
			Instance._musicSource.VolumeDb = volume;
			Instance._shadowSource.VolumeDb = volume;
		}

		public static void PlayMusicByName(string name = "")
		{
			_musicName = name;
		}

		public override void _Ready()
		{
			Instance = this;
			AudioManager.Instance.SetAudioSettings(audioSettings);
			shadowFormEQCurve = new Curve();
			shadowFormEQCurve.AddPoint(new Vector2(0f, 0f));
			shadowFormEQCurve.AddPoint(new Vector2(1f, 1f));
			_activateShadow = false;
			_isActivateShadow = false;
			_shadowSource = CreateAudioSource(shadowFormAmbient);
			_musicSource = CreateAudioSource(null);
			_musicSource.Play();
		}

		public override void _ExitTree()
		{
		}

		public override void _Process(double delta)
		{
			if (_activateShadow && NeedSpecialShadowFormSound)
			{
				_timer += GameTimeController.deltaTime;
				SetShadowProgerss(_timer);
			}
		}

		private void SetShadowProgerss(float timer)
		{
			float num = shadowFormEQCurve.Sample(timer / eqAnimationTime);
			if (!_isActivateShadow)
			{
				num = 1f - num;
			}
			if (timer >= eqAnimationTime)
			{
				_activateShadow = false;
			}
		}

		private AudioStreamPlayer3D CreateAudioSource(AudioStream clip = null)
		{
			AudioStreamPlayer3D audioSource = new AudioStreamPlayer3D();
			audioSource.Name = "Audio_" + ((clip != null) ? clip.ResourceName : string.Empty);
			audioSource.Stream = clip;
			audioSource.VolumeDb = AudioManager.musicVolume;
			AddChild(audioSource);
			return audioSource;
		}

		public void ActivateShadowFormEQSetting(bool activate)
		{
			if (activate && !_shadowSource.Playing && NeedSpecialShadowFormSound)
			{
				_shadowSource.Play();
			}
			else if (!activate && _shadowSource.Playing)
			{
				_shadowSource.Stop();
			}
			_activateShadow = true;
			_isActivateShadow = activate;
		}

		private AudioStream GetAudioStream()
		{
			return null;
		}
	}
}
