using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Effects;
using Godot;
using Nekki.Yaml;
namespace SF3.Moves
{
	public partial class TriggerActionShakeCamera3D : TriggerAction
	{
		private readonly Vector3 _amplitude;
		private readonly RpnValue<int> _duration;
		private readonly Vector3 _period;
		public Vector3 Amplitude
		{
			get
			{
				return _amplitude;
			}
		}
		public int Duration
		{
			get
			{
				return _duration;
			}
		}
		public Vector3 Period
		{
			get
			{
				return _period;
			}
		}
		public TriggerActionShakeCamera3D(Node yamlNode)
			: base(EActionType.SHAKE_CAMERA, yamlNode)
		{
			if (!(yamlNode is Scalar))
			{
				string outResult;
				if (TryGetString(out outResult, "Frames", string.Empty, string.Empty, null, false))
				{
					_duration = outResult;
				}
				else
				{
					_duration = 0;
				}
				TryGetFloat(out _amplitude.x, "AmpX", 0f, string.Empty, null, false);
				TryGetFloat(out _amplitude.y, "AmpY", 0f, string.Empty, null, false);
				TryGetFloat(out _amplitude.z, "AmpZ", 0f, string.Empty, null, false);
				TryGetFloat(out _period.x, "PeriodX", 0f, string.Empty, null, false);
				TryGetFloat(out _period.y, "PeriodY", 0f, string.Empty, null, false);
				TryGetFloat(out _period.z, "PeriodZ", 0f, string.Empty, null, false);
			}
		}
		protected override void ApplyAction(object modelData)
		{
			base.ApplyAction(modelData);
			EffectsManager.PlayEffect(base.name, this);
		}
	}
}

