using Godot;
using Nekki.Yaml;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionToggleShadowForm : TriggerAction
	{
		public TriggerActionToggleShadowForm(Node yamlNode) : base(EActionType.TOGGLE_SHADOW_FORM, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
