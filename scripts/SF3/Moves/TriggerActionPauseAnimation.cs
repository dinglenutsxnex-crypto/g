using Nekki.Yaml;
using SF3.GameModels;
namespace SF3.Moves
{
	public partial class TriggerActionPauseAnimationPlayer : TriggerAction
	{
		private readonly int _inOutFrames;
		private readonly int _forFrames;
		public TriggerActionPauseAnimationPlayer(Node yamlNode)
			: base(EActionType.PAUSE_AnimationPlayer, yamlNode)
		{
			base.name = base.name.ToLower();
			TryGetInt(out _inOutFrames, "InOutFrames", 0, string.Empty, this);
			TryGetInt(out _forFrames, "Frames", 0, string.Empty, this);
		}
		protected override void ApplyAction(object modelData)
		{
			((Model)modelData).modelAnimationPlayer.Pause(_forFrames, _inOutFrames, _forFrames);
		}
	}
}

