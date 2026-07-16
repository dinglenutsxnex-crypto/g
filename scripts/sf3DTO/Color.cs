using System;

namespace sf3DTO
{
	public sealed partial class Color : IEquatable<Color>, ICloneable
	{
		public int ColorId { get; set; }
		public double Value { get; set; }

		public Color()
		{
		}

		public Color(Color other)
		{
			ColorId = other.ColorId;
			Value = other.Value;
		}

		public object Clone()
		{
			return new Color(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Color);
		}

		public bool Equals(Color other)
		{
			if (ReferenceEquals(other, null))
			{
				return false;
			}
			if (ReferenceEquals(other, this))
			{
				return true;
			}
			if (ColorId != other.ColorId)
			{
				return false;
			}
			if (Value != other.Value)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (ColorId != 0)
			{
				num ^= ColorId.GetHashCode();
			}
			if (Value != 0.0)
			{
				num ^= Value.GetHashCode();
			}
			return num;
		}

		public override string ToString()
		{
			return string.Format("Color: ColorId={0}, Value={1}", ColorId, Value);
		}
	}
}
