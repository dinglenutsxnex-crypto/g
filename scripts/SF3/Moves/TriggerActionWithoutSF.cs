using Godot;
using Nekki.Yaml;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionWithoutSF : TriggerAction
	{
		public TriggerActionWithoutSF(Node yamlNode) : base(EActionType.WITHOUT_SF, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
