using Nekki.Yaml;
namespace SF3.Moves
{
	public partial class TriggerEventAnimationEnd : TriggerEvent
	{
		public TriggerEventAnimationEnd(Mapping eventMap)
			: base(ETriggerEvents.EVENT_ANIMATION_END, eventMap)
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

