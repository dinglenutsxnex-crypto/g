using System;
using System.Collections.Generic;
using System.Linq;

namespace sf3DTO
{
	public sealed class Inventory : IEquatable<Inventory>, ICloneable
	{
		public List<Item> Items { get; set; }

		public List<Perk> Perks { get; set; }

		public List<Booster> Boosters { get; set; }

		public Inventory()
		{
			Items = new List<Item>();
			Perks = new List<Perk>();
			Boosters = new List<Booster>();
		}

		public Inventory(Inventory other)
		{
			Items = other.Items?.ToList() ?? new List<Item>();
			Perks = other.Perks?.ToList() ?? new List<Perk>();
			Boosters = other.Boosters?.ToList() ?? new List<Booster>();
		}

		public object Clone()
		{
			return new Inventory(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Inventory);
		}

		public bool Equals(Inventory other)
		{
			if (ReferenceEquals(other, null))
			{
				return false;
			}
			if (ReferenceEquals(other, this))
			{
				return true;
			}
			if (!Items.SequenceEqual(other.Items))
			{
				return false;
			}
			if (!Perks.SequenceEqual(other.Perks))
			{
				return false;
			}
			if (!Boosters.SequenceEqual(other.Boosters))
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			num ^= Items.GetHashCode();
			num ^= Perks.GetHashCode();
			return num ^ Boosters.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Inventory[Items={0}, Perks={1}, Boosters={2}]", Items.Count, Perks.Count, Boosters.Count);
		}
	}
}
