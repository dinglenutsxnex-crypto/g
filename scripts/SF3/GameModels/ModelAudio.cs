using Godot;

namespace SF3.GameModels
{
	public class ModelAudio
	{
		private AudioStreamPlayer3D _modelAudio;

		public ModelAudio(AudioStreamPlayer3D audio)
		{
			_modelAudio = audio;
			_modelAudio.Autoplay = false;
		}

		public void PlaySound(AudioStream clip)
		{
			_modelAudio.Stream = clip;
			_modelAudio.Play();
		}
	}
}
