using Godot;
using Nekki.Yaml;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionInvisibleWarrior : TriggerAction
	{
		public TriggerActionInvisibleWarrior(Node yamlNode) : base(EActionType.INVISIBLE_WARRIOR, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
