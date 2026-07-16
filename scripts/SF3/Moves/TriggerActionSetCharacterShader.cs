using Godot;
using Nekki.Yaml;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionSetCharacterShader : TriggerAction
	{
		public TriggerActionSetCharacterShader(Node yamlNode) : base(EActionType.SET_CHARACTER_SHADER, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
