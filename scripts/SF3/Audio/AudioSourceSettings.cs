using System;
using Godot;

namespace SF3.Audio
{
	[Serializable]
	public class AudioSourceSettings
	{
		[Serializable]
		public class AudioCurve
		{
			public bool apply;
			public Curve curve = new Curve();
		}

		public float dopplerLevel = 1f;
		public float spread;
		public float minDistance = 1f;
		public float maxDistance = 500f;
		public AudioCurve volumeCurve;
		public AudioCurve spatialCurve;
		public AudioCurve spreadCurve;
		public AudioCurve reverbCurve;

		public void ApplySettings(AudioStreamPlayer3D applyTo)
		{
			applyTo.MaxDistance = maxDistance;
			// Godot AudioStreamPlayer3D does not have direct doppler/spread/rolloff equivalents at same granularity
		}
	}
}
