using Godot;
using Nekki.Yaml;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionDestroyMe : TriggerAction
	{
		public TriggerActionDestroyMe(Node yamlNode) : base(EActionType.DESTROY_ME, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
