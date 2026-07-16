using Godot;
using Nekki.Yaml;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerDestroyEffect : TriggerAction
	{
		public TriggerDestroyEffect(Node yamlNode) : base(EActionType.DESTROY_EFFECT, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
