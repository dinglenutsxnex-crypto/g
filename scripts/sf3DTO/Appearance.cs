using System;

namespace sf3DTO
{
	public sealed class Appearance : IEquatable<Appearance>, ICloneable
	{
		public Gender Gender { get; set; }

		public int HeadId { get; set; }

		public Color HairColor { get; set; }

		public Color SkinColor { get; set; }

		public Appearance()
		{
		}

		public Appearance(Appearance other)
		{
			Gender = other.Gender;
			HeadId = other.HeadId;
			HairColor = other.HairColor?.Clone() as Color;
			SkinColor = other.SkinColor?.Clone() as Color;
		}

		public object Clone()
		{
			return new Appearance(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Appearance);
		}

		public bool Equals(Appearance other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (Gender != other.Gender) return false;
			if (HeadId != other.HeadId) return false;
			if (!Equals(HairColor, other.HairColor)) return false;
			if (!Equals(SkinColor, other.SkinColor)) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (Gender != 0) num ^= Gender.GetHashCode();
			if (HeadId != 0) num ^= HeadId.GetHashCode();
			if (HairColor != null) num ^= HairColor.GetHashCode();
			if (SkinColor != null) num ^= SkinColor.GetHashCode();
			return num;
		}

		public override string ToString()
		{
			return string.Format("Appearance[Gender={0}, HeadId={1}]", Gender, HeadId);
		}
	}
}
