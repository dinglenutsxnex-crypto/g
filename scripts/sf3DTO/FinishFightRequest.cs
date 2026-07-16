using System;
using System.Collections.Generic;

namespace sf3DTO
{
	public sealed class FinishFightRequest : IEquatable<FinishFightRequest>, ICloneable
	{
		public int BattleModelId { get; set; }

		public int BattleCounter { get; set; }

		public int CurrentFightIndex { get; set; }

		public FightResult Result { get; set; }

		public int WonRounds { get; set; }

		public Timestamp FinishTime { get; set; }

		public int TotalRounds { get; set; }

		public List<FinishFightRewardMultiplier> Multipliers { get; set; } = new List<FinishFightRewardMultiplier>();

		public FinishFightRequest()
		{
		}

		public FinishFightRequest(FinishFightRequest other)
		{
			BattleModelId = other.BattleModelId;
			BattleCounter = other.BattleCounter;
			CurrentFightIndex = other.CurrentFightIndex;
			Result = other.Result;
			WonRounds = other.WonRounds;
			FinishTime = other.FinishTime?.Clone() as Timestamp;
			TotalRounds = other.TotalRounds;
			Multipliers = new List<FinishFightRewardMultiplier>(other.Multipliers);
		}

		public object Clone()
		{
			return new FinishFightRequest(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as FinishFightRequest);
		}

		public bool Equals(FinishFightRequest other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (BattleModelId != other.BattleModelId) return false;
			if (BattleCounter != other.BattleCounter) return false;
			if (CurrentFightIndex != other.CurrentFightIndex) return false;
			if (Result != other.Result) return false;
			if (WonRounds != other.WonRounds) return false;
			if (!Equals(FinishTime, other.FinishTime)) return false;
			if (TotalRounds != other.TotalRounds) return false;
			if (!Multipliers.Equals(other.Multipliers)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (BattleModelId != 0) num ^= BattleModelId.GetHashCode();
			if (BattleCounter != 0) num ^= BattleCounter.GetHashCode();
			if (CurrentFightIndex != 0) num ^= CurrentFightIndex.GetHashCode();
			if (Result != 0) num ^= Result.GetHashCode();
			if (WonRounds != 0) num ^= WonRounds.GetHashCode();
			if (FinishTime != null) num ^= FinishTime.GetHashCode();
			if (TotalRounds != 0) num ^= TotalRounds.GetHashCode();
			return num ^ Multipliers.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("FinishFightRequest[BattleModelId={0}, Result={1}]", BattleModelId, Result);
		}
	}
}
