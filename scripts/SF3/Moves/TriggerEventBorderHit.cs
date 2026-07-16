using Godot;
using System;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;

namespace SF3.Moves
{
	public class TriggerEventBorderHit : TriggerEvent
	{
		public TriggerEventBorderHit(Mapping eventMap)
			: base(ETriggerEvents.EVENT_BORDER_HIT, eventMap)
		{
		}

		public override bool Equal(BattleEventArgs args)
		{
			if (!base.Equal(args))
			{
				return false;
			}
			return true;
		}
	}
}
