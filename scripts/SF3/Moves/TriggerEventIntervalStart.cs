using Nekki.Yaml;
namespace SF3.Moves
{
	public partial class TriggerEventIntervalStart : TriggerEvent
	{
		public TriggerEventIntervalStart(Mapping eventMap)
			: base(ETriggerEvents.EVENT_INTERVAL_START, eventMap)
		{
		}
		public override bool Equal(BattleEventArgs args)
		{
			if (!base.Equal(args))
			{
				return false;
			}
			IntervalAnimationPlayer intervalAnimationPlayer = (IntervalAnimationPlayer)args.EventData;
			return name.Length == 0 || name.Equals(intervalAnimationPlayer.name);
		}
	}
}

