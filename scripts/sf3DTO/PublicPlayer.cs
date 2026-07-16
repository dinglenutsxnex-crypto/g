using System;

namespace sf3DTO
{
    public sealed class PublicPlayer : IEquatable<PublicPlayer>
    {
        private ShortPlayer shortPlayer_;

        public ShortPlayer ShortPlayer { get => shortPlayer_; set => shortPlayer_ = value; }

        public PublicPlayer() { }

        public PublicPlayer(PublicPlayer other)
        {
            ShortPlayer = other.shortPlayer_?.Clone();
        }

        public PublicPlayer Clone() => new PublicPlayer(this);

        public override bool Equals(object other) => Equals(other as PublicPlayer);

        public bool Equals(PublicPlayer other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Equals(ShortPlayer, other.ShortPlayer);
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (shortPlayer_ != null) num ^= ShortPlayer.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{ShortPlayer: {ShortPlayer}}}";
        }
    }
}
