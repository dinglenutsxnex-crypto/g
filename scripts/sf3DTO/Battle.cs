using System;
using System.Collections.Generic;
namespace sf3DTO
{
	public class Battle
	{
		public List<GeneratedBattle> Battles { get; set; } = new List<GeneratedBattle>();
		public int BattleCounter { get; set; }
		public int CurrentFightIndex { get; set; }
		public DateTime? GenTime { get; set; }
		public DateTime? LastFightFinishTime { get; set; }
	}
}
