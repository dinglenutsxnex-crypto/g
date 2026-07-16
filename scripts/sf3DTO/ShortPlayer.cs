using System;

namespace sf3DTO
{
    public sealed class ShortPlayer : IEquatable<ShortPlayer>
    {
        private long playerId_;
        private string nickname_ = string.Empty;
        private string displayName_ = string.Empty;
        private int level_;

        public long PlayerId { get => playerId_; set => playerId_ = value; }
        public string Nickname { get => nickname_; set => nickname_ = value ?? string.Empty; }
        public string DisplayName { get => displayName_; set => displayName_ = value ?? string.Empty; }
        public int Level { get => level_; set => level_ = value; }

        public ShortPlayer() { }

        public ShortPlayer(ShortPlayer other)
        {
            playerId_ = other.playerId_;
            nickname_ = other.nickname_;
            displayName_ = other.displayName_;
            level_ = other.level_;
        }

        public ShortPlayer Clone() => new ShortPlayer(this);

        public override bool Equals(object other) => Equals(other as ShortPlayer);

        public bool Equals(ShortPlayer other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return PlayerId == other.PlayerId && Nickname == other.Nickname && DisplayName == other.DisplayName && Level == other.Level;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (PlayerId != 0) num ^= PlayerId.GetHashCode();
            if (Nickname.Length != 0) num ^= Nickname.GetHashCode();
            if (DisplayName.Length != 0) num ^= DisplayName.GetHashCode();
            if (Level != 0) num ^= Level.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{PlayerId: {PlayerId}, Nickname: {Nickname}, DisplayName: {DisplayName}, Level: {Level}}}";
        }
    }
}
