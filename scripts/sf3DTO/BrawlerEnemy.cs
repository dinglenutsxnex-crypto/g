using System;
using System.Collections.Generic;

namespace sf3DTO
{
	public sealed class BrawlerEnemy : IEquatable<BrawlerEnemy>, ICloneable
	{
		public ShortPlayer ShortPlayer { get; set; }

		public List<Item> Items { get; set; } = new List<Item>();

		public List<Perk> Perks { get; set; } = new List<Perk>();

		public Appearance Appearance { get; set; }

		public BrawlerEnemy()
		{
		}

		public BrawlerEnemy(BrawlerEnemy other)
		{
			ShortPlayer = other.ShortPlayer?.Clone() as ShortPlayer;
			Items = new List<Item>(other.Items);
			Perks = new List<Perk>(other.Perks);
			Appearance = other.Appearance?.Clone() as Appearance;
		}

		public object Clone()
		{
			return new BrawlerEnemy(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as BrawlerEnemy);
		}

		public bool Equals(BrawlerEnemy other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (!Equals(ShortPlayer, other.ShortPlayer)) return false;
			if (!Items.Equals(other.Items)) return false;
			if (!Perks.Equals(other.Perks)) return false;
			if (!Equals(Appearance, other.Appearance)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (ShortPlayer != null) num ^= ShortPlayer.GetHashCode();
			num ^= Items.GetHashCode();
			num ^= Perks.GetHashCode();
			if (Appearance != null) num ^= Appearance.GetHashCode();
			return num;
		}

		public override string ToString()
		{
			return string.Format("BrawlerEnemy[ShortPlayer={0}]", ShortPlayer);
		}
	}
}
