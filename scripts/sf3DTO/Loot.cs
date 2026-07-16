using System;
using System.Collections.Generic;
using System.Linq;

namespace sf3DTO
{
	public sealed class Loot : IEquatable<Loot>, ICloneable
	{
		public List<Currency> Currencies { get; set; }

		public long Experience { get; set; }

		public List<Item> Equipments { get; set; }

		public List<Perk> Perks { get; set; }

		public List<Booster> Boosters { get; set; }

		public Loot()
		{
			Currencies = new List<Currency>();
			Equipments = new List<Item>();
			Perks = new List<Perk>();
			Boosters = new List<Booster>();
		}

		public Loot(Loot other)
		{
			Currencies = other.Currencies?.ToList() ?? new List<Currency>();
			Experience = other.Experience;
			Equipments = other.Equipments?.ToList() ?? new List<Item>();
			Perks = other.Perks?.ToList() ?? new List<Perk>();
			Boosters = other.Boosters?.ToList() ?? new List<Booster>();
		}

		public object Clone()
		{
			return new Loot(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Loot);
		}

		public bool Equals(Loot other)
		{
			if (ReferenceEquals(other, null))
			{
				return false;
			}
			if (ReferenceEquals(other, this))
			{
				return true;
			}
			if (!Currencies.SequenceEqual(other.Currencies))
			{
				return false;
			}
			if (Experience != other.Experience)
			{
				return false;
			}
			if (!Equipments.SequenceEqual(other.Equipments))
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
			num ^= Currencies.GetHashCode();
			if (Experience != 0)
			{
				num ^= Experience.GetHashCode();
			}
			num ^= Equipments.GetHashCode();
			num ^= Perks.GetHashCode();
			return num ^ Boosters.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Loot[Currencies={0}, Experience={1}, Equipments={2}, Perks={3}, Boosters={4}]",
				Currencies.Count, Experience, Equipments.Count, Perks.Count, Boosters.Count);
		}
	}
}
