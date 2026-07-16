using Godot;
using Nekki.Yaml;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionWithoutHP : TriggerAction
	{
		public TriggerActionWithoutHP(Node yamlNode) : base(EActionType.WITHOUT_HP, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
