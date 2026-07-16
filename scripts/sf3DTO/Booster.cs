using System;

namespace sf3DTO
{
	public sealed class Booster : IEquatable<Booster>, ICloneable
	{
		public long InstanceId { get; set; }

		public int ModelId { get; set; }

		public Loot Loot { get; set; }

		public Booster()
		{
		}

		public Booster(Booster other)
		{
			InstanceId = other.InstanceId;
			ModelId = other.ModelId;
			Loot = other.Loot?.Clone() as Loot;
		}

		public object Clone()
		{
			return new Booster(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Booster);
		}

		public bool Equals(Booster other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (InstanceId != other.InstanceId) return false;
			if (ModelId != other.ModelId) return false;
			return Equals(Loot, other.Loot);
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (InstanceId != 0) num ^= InstanceId.GetHashCode();
			if (ModelId != 0) num ^= ModelId.GetHashCode();
			if (Loot != null) num ^= Loot.GetHashCode();
			return num;
		}

		public override string ToString()
		{
			return string.Format("Booster[InstanceId={0}, ModelId={1}, Loot={2}]", InstanceId, ModelId, Loot);
		}
	}
}
