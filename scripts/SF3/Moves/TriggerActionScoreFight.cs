using Godot;
using Nekki.Yaml;
using SF3.Moves;

namespace SF3.Moves
{
	public class TriggerActionScoreFight : TriggerAction
	{
		public TriggerActionScoreFight(Node yamlNode) : base(EActionType.SCORE_FIGHT, yamlNode) { }

		protected override void ApplyAction(object modelData) { }
	}
}
