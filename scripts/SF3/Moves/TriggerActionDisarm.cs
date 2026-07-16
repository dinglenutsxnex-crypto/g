using Godot;
using Nekki.Yaml;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionDisarm : TriggerAction
	{
		public TriggerActionDisarm(Node yamlNode) : base(EActionType.DISARM, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
