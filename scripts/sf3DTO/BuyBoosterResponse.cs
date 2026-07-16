using System;
using System.Collections.Generic;

namespace sf3DTO
{
	public sealed class BuyBoosterResponse : IEquatable<BuyBoosterResponse>, ICloneable
	{
		public Booster Booster { get; set; }

		public List<Currency> Currency { get; set; } = new List<Currency>();

		public BuyBoosterResponse()
		{
		}

		public BuyBoosterResponse(BuyBoosterResponse other)
		{
			Booster = other.Booster?.Clone() as Booster;
			Currency = new List<Currency>(other.Currency);
		}

		public object Clone()
		{
			return new BuyBoosterResponse(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as BuyBoosterResponse);
		}

		public bool Equals(BuyBoosterResponse other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (!Equals(Booster, other.Booster)) return false;
			if (!Currency.Equals(other.Currency)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (Booster != null) num ^= Booster.GetHashCode();
			return num ^ Currency.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("BuyBoosterResponse[Booster={0}, Currency={1}]", Booster, Currency);
		}
	}
}
