using Godot;
using Nekki.Yaml;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionSetShadowChargeBurnDown : TriggerAction
	{
		public TriggerActionSetShadowChargeBurnDown(Node yamlNode) : base(EActionType.SET_SHADOW_CHARGE_BURN_DOWN, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
