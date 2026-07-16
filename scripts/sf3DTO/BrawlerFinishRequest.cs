using System;
using System.Collections.Generic;

namespace sf3DTO
{
	public sealed class BrawlerFinishRequest : IEquatable<BrawlerFinishRequest>, ICloneable
	{
		public BrawlerEnemy Enemy { get; set; }

		public FightResult Result { get; set; }

		public int TotalRounds { get; set; }

		public List<FinishFightRewardMultiplier> Multipliers { get; set; } = new List<FinishFightRewardMultiplier>();

		public BrawlerFinishRequest()
		{
		}

		public BrawlerFinishRequest(BrawlerFinishRequest other)
		{
			Enemy = other.Enemy?.Clone() as BrawlerEnemy;
			Result = other.Result;
			TotalRounds = other.TotalRounds;
			Multipliers = new List<FinishFightRewardMultiplier>(other.Multipliers);
		}

		public object Clone()
		{
			return new BrawlerFinishRequest(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as BrawlerFinishRequest);
		}

		public bool Equals(BrawlerFinishRequest other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (!Equals(Enemy, other.Enemy)) return false;
			if (Result != other.Result) return false;
			if (TotalRounds != other.TotalRounds) return false;
			if (!Multipliers.Equals(other.Multipliers)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (Enemy != null) num ^= Enemy.GetHashCode();
			if (Result != 0) num ^= Result.GetHashCode();
			if (TotalRounds != 0) num ^= TotalRounds.GetHashCode();
			return num ^ Multipliers.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("BrawlerFinishRequest[Enemy={0}, Result={1}, TotalRounds={2}]", Enemy, Result, TotalRounds);
		}
	}
}
