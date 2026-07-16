using Godot;
using Nekki.Yaml;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionDisableRepulsion : TriggerAction
	{
		public TriggerActionDisableRepulsion(Node yamlNode) : base(EActionType.DISABLE_REPULSION, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
