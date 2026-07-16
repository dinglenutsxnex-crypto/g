using System;
using Google.Protobuf.WellKnownTypes;

namespace sf3DTO
{
	public sealed class ExtendedPlayer : IEquatable<ExtendedPlayer>, ICloneable
	{
		public Player PrimaryPlayer { get; set; }

		public Player SecondaryPlayer { get; set; }

		public Account PrimaryAccount { get; set; }

		public Account SecondaryAccount { get; set; }

		public Timestamp Timestamp { get; set; }

		public BrawlerFinish BrawlerFinish { get; set; }

		public ExtendedPlayer()
		{
		}

		public ExtendedPlayer(ExtendedPlayer other)
		{
			PrimaryPlayer = other.PrimaryPlayer?.Clone() as Player;
			SecondaryPlayer = other.SecondaryPlayer?.Clone() as Player;
			PrimaryAccount = other.PrimaryAccount?.Clone() as Account;
			SecondaryAccount = other.SecondaryAccount?.Clone() as Account;
			Timestamp = other.Timestamp?.Clone() as Timestamp;
			BrawlerFinish = other.BrawlerFinish?.Clone() as BrawlerFinish;
		}

		public object Clone()
		{
			return new ExtendedPlayer(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as ExtendedPlayer);
		}

		public bool Equals(ExtendedPlayer other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (!Equals(PrimaryPlayer, other.PrimaryPlayer)) return false;
			if (!Equals(SecondaryPlayer, other.SecondaryPlayer)) return false;
			if (!Equals(PrimaryAccount, other.PrimaryAccount)) return false;
			if (!Equals(SecondaryAccount, other.SecondaryAccount)) return false;
			if (!Equals(Timestamp, other.Timestamp)) return false;
			if (!Equals(BrawlerFinish, other.BrawlerFinish)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (PrimaryPlayer != null) num ^= PrimaryPlayer.GetHashCode();
			if (SecondaryPlayer != null) num ^= SecondaryPlayer.GetHashCode();
			if (PrimaryAccount != null) num ^= PrimaryAccount.GetHashCode();
			if (SecondaryAccount != null) num ^= SecondaryAccount.GetHashCode();
			if (Timestamp != null) num ^= Timestamp.GetHashCode();
			if (BrawlerFinish != null) num ^= BrawlerFinish.GetHashCode();
			return num;
		}

		public override string ToString()
		{
			return string.Format("ExtendedPlayer[PrimaryPlayer={0}]", PrimaryPlayer);
		}
	}
}
