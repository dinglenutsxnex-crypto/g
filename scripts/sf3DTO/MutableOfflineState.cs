using System;
using System.Collections.Generic;

namespace sf3DTO
{
	public sealed class MutableOfflineState : IEquatable<MutableOfflineState>, ICloneable
	{
		public long StateId { get; set; }

		public long Experience { get; set; }

		public int Level { get; set; }

		public List<Currency> Currencies { get; set; } = new List<Currency>();

		public Inventory Inventory { get; set; }

		public MutableOfflineState()
		{
		}

		public MutableOfflineState(MutableOfflineState other)
		{
			StateId = other.StateId;
			Experience = other.Experience;
			Level = other.Level;
			Currencies = new List<Currency>(other.Currencies);
			Inventory = other.Inventory?.Clone() as Inventory;
		}

		public object Clone()
		{
			return new MutableOfflineState(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as MutableOfflineState);
		}

		public bool Equals(MutableOfflineState other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (StateId != other.StateId) return false;
			if (Experience != other.Experience) return false;
			if (Level != other.Level) return false;
			if (!Currencies.Equals(other.Currencies)) return false;
			if (!Equals(Inventory, other.Inventory)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (StateId != 0) num ^= StateId.GetHashCode();
			if (Experience != 0) num ^= Experience.GetHashCode();
			if (Level != 0) num ^= Level.GetHashCode();
			num ^= Currencies.GetHashCode();
			if (Inventory != null) num ^= Inventory.GetHashCode();
			return num;
		}

		public override string ToString()
		{
			return string.Format("MutableOfflineState[StateId={0}, Experience={1}, Level={2}]", StateId, Experience, Level);
		}
	}
}
