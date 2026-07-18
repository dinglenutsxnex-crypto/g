using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.GameModels;
namespace SF3.Moves
{
	public partial class TriggerActionAnimationPlayer : TriggerAction
	{
		public TriggerActionAnimationPlayer(Node yamNode)
			: base(EActionType.ANIMATION, yamNode)
		{
			base.name = base.name.ToLower();
		}
		protected override void ApplyAction(object modelData)
		{
			((Model)modelData).modelAnimationPlayer.Play(base.name);
		}
	}
}

