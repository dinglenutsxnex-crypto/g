using Godot;
using Nekki.Yaml;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerGUIEffect : TriggerAction
	{
		public TriggerGUIEffect(Node yamlNode) : base(EActionType.GUI_EFFECT, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
