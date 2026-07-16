using System;
using System.Collections.Generic;

namespace sf3DTO
{
    public sealed class ShortPlayers : IEquatable<ShortPlayers>
    {
        private List<ShortPlayer> values_ = new List<ShortPlayer>();

        public List<ShortPlayer> Values => values_;

        public ShortPlayers() { }

        public ShortPlayers(ShortPlayers other)
        {
            values_ = new List<ShortPlayer>(other.values_);
        }

        public ShortPlayers Clone() => new ShortPlayers(this);

        public override bool Equals(object other) => Equals(other as ShortPlayers);

        public bool Equals(ShortPlayers other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return true;
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public override string ToString()
        {
            return $"{{Values count: {values_.Count}}}";
        }
    }
}
