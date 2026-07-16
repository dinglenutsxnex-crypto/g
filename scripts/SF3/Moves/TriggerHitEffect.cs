using Godot;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerHitEffect : TriggerAction
	{
		public TriggerHitEffect(Node yamlNode) : base(EActionType.HIT_EFFECT, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
