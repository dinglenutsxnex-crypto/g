using Godot;
using System;
using SF3.Audio;

namespace SF3.Effects
{
	public class PlaySoundEffect : IGameEffect
	{
		private string clipName;

		public void Create()
		{
			AudioManager.Instance.LoadSound(clipName);
		}

		public void Initialize()
		{
		}

		public void Play()
		{
			AudioManager.Instance.PlaySound(clipName);
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
	}
}
