using System;
using System.Collections.Generic;

namespace sf3DTO
{
	public sealed class Warrior : IEquatable<Warrior>, ICloneable
	{
		public string Alias { get; set; } = string.Empty;

		public Gender Gender { get; set; }

		public int AppearanceId { get; set; }

		public AiMode AiMode { get; set; }

		public double Power { get; set; }

		public List<WarriorItemId> Equipments { get; set; } = new List<WarriorItemId>();

		public List<Perk> Perks { get; set; } = new List<Perk>();

		public Warrior()
		{
		}

		public Warrior(Warrior other)
		{
			Alias = other.Alias;
			Gender = other.Gender;
			AppearanceId = other.AppearanceId;
			AiMode = other.AiMode;
			Power = other.Power;
			Equipments = new List<WarriorItemId>(other.Equipments);
			Perks = new List<Perk>(other.Perks);
		}

		public object Clone()
		{
			return new Warrior(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Warrior);
		}

		public bool Equals(Warrior other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (Alias != other.Alias) return false;
			if (Gender != other.Gender) return false;
			if (AppearanceId != other.AppearanceId) return false;
			if (AiMode != other.AiMode) return false;
			if (Power != other.Power) return false;
			if (!Equipments.Equals(other.Equipments)) return false;
			if (!Perks.Equals(other.Perks)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (Alias.Length != 0) num ^= Alias.GetHashCode();
			if (Gender != 0) num ^= Gender.GetHashCode();
			if (AppearanceId != 0) num ^= AppearanceId.GetHashCode();
			if (AiMode != 0) num ^= AiMode.GetHashCode();
			if (Power != 0.0) num ^= Power.GetHashCode();
			num ^= Equipments.GetHashCode();
			return num ^ Perks.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Warrior[Alias={0}, Gender={1}, Power={2}]", Alias, Gender, Power);
		}
	}
}
