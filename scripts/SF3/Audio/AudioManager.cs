using System.Collections.Generic;
using Godot;

namespace SF3.Audio
{
	public partial class AudioManager : Node
	{
		private Dictionary<string, AudioStreamPlayer3D> sources;
		private List<Node> sounds;
		private AudioSourceSettings audioSettings;
		private static bool muted;
		private static float pitch;

		public static AudioManager Instance { get; private set; }
		public static float volume { get; private set; }
		public static float musicVolume { get; private set; }

		public static void Initialize()
		{
			if (Instance == null)
			{
				Instance = new AudioManager();
				Instance.Name = "AudioManager";
				StaticObjectsManager.AddObject(Instance);
				Instance.sounds = new List<Node>();
				Instance.sources = new Dictionary<string, AudioStreamPlayer3D>();
				muted = false;
				volume = 1f;
				musicVolume = 1f;
				pitch = 1f;
			}
		}

		public void SetAudioSettings(AudioSourceSettings sourceSettings)
		{
			audioSettings = sourceSettings;
			foreach (AudioStreamPlayer3D value in sources.Values)
			{
				audioSettings.ApplySettings(value);
			}
		}

		public void SetPitch(float pitchValue)
		{
		}

		public void Mute(bool mute)
		{
		}

		public void SetVolume(float volumeValue, bool save = false)
		{
			volume = volumeValue;
			foreach (AudioStreamPlayer3D value in sources.Values)
			{
				value.VolumeDb = volume;
			}
		}

		public void SetMusicVolume(float volumeValue)
		{
			musicVolume = volumeValue;
			LocationAudioSettings.SetVolume(volumeValue);
		}

		public void LoadSound(params string[] sounds)
		{
		}

		public AudioStreamPlayer3D LoadSound(string clipName)
		{
			return null;
		}

		public void PlaySound(string audioName, float spatialBlend = 0f)
		{
		}

		public void PlaySound(string audioName, Vector3 pos, float spatialBlend = 0f)
		{
		}

		public void ClearAllSound()
		{
			foreach (Node sound in sounds)
			{
				sound.QueueFree();
			}
			sources.Clear();
			sounds.Clear();
		}
	}
}
