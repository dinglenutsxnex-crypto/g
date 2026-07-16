using Nekki.Yaml;
namespace SF3.Moves
{
	public partial class TriggerEventAnimationStart : TriggerEvent
	{
		public TriggerEventAnimationStart(Mapping eventMap)
			: base(ETriggerEvents.EVENT_ANIMATION_START, eventMap)
		{
			name = name.ToLower();
		}
		public override bool Equal(BattleEventArgs args)
		{
			if (!base.Equal(args))
			{
				return false;
			}
			ModelInfoAnimationPlayer modelInfoAnimationPlayer = (ModelInfoAnimationPlayer)args.EventData;
			return name.Length == 0 || modelInfoAnimationPlayer.AnimationPlayer.HasNameOrGroup(name);
		}
	}
}

