using System.Collections.Generic;
using SF3.Moves;
public partial class MovesData
{
	public Dictionary<string, InfoAnimationPlayer> Animations = new Dictionary<string, InfoAnimationPlayer>();
	public Dictionary<string, AnimationBinaries> Binaries = new Dictionary<string, AnimationBinaries>();
	public Dictionary<string, InfoTriggerSet> Triggers = new Dictionary<string, InfoTriggerSet>();
}

