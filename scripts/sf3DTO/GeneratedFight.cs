using System;
using System.Collections.Generic;
namespace sf3DTO
{
	public class GeneratedFight
	{
		public List<GeneratedRound> Rounds { get; set; } = new List<GeneratedRound>();
		public List<Loot> RewardsByRoundWins { get; set; } = new List<Loot>();
	}
}
