using System;

namespace sf3DTO
{
	public sealed class CreatePlayerRequest : IEquatable<CreatePlayerRequest>, ICloneable
	{
		public string DisplayName { get; set; } = string.Empty;

		public Appearance Appearance { get; set; }

		public string Version { get; set; } = string.Empty;

		public CreatePlayerRequest()
		{
		}

		public CreatePlayerRequest(CreatePlayerRequest other)
		{
			DisplayName = other.DisplayName;
			Appearance = other.Appearance?.Clone() as Appearance;
			Version = other.Version;
		}

		public object Clone()
		{
			return new CreatePlayerRequest(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as CreatePlayerRequest);
		}

		public bool Equals(CreatePlayerRequest other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (DisplayName != other.DisplayName) return false;
			if (!Equals(Appearance, other.Appearance)) return false;
			if (Version != other.Version) return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (DisplayName.Length != 0) num ^= DisplayName.GetHashCode();
			if (Appearance != null) num ^= Appearance.GetHashCode();
			if (Version.Length != 0) num ^= Version.GetHashCode();
			return num;
		}

		public override string ToString()
		{
			return string.Format("CreatePlayerRequest[DisplayName={0}, Appearance={1}, Version={2}]", DisplayName, Appearance, Version);
		}
	}
}
