using Godot;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionSetShadowCharge : TriggerAction
	{
		public TriggerActionSetShadowCharge(Node yamlNode) : base(EActionType.SET_SHADOW_CHARGE, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
