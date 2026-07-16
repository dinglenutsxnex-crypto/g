using System;
using System.Collections.Generic;
namespace sf3DTO
{
	public class Shop
	{
		public List<ShopItem> Items { get; set; } = new List<ShopItem>();
		public DateTime? LastGenerationTime { get; set; }
	}
}
