using System;
using System.Collections.Generic;
namespace sf3DTO
{
	public class Item
	{
		public int ModelId { get; set; }
		public double StackLevel { get; set; }
		public bool Equipped { get; set; }
		public List<PerkSlot> Perks { get; set; } = new List<PerkSlot>();
	}
}
