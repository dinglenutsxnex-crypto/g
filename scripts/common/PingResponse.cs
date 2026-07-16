using System;

namespace common
{
    public sealed class PingResponse : IEquatable<PingResponse>
    {
        private Timestamp clientTimestamp_;
        private Timestamp serverTimestamp_;

        public Timestamp ClientTimestamp { get => clientTimestamp_; set => clientTimestamp_ = value; }
        public Timestamp ServerTimestamp { get => serverTimestamp_; set => serverTimestamp_ = value; }

        public PingResponse() { }

        public PingResponse(PingResponse other)
        {
            ClientTimestamp = other.clientTimestamp_?.Clone();
            ServerTimestamp = other.serverTimestamp_?.Clone();
        }

        public PingResponse Clone() => new PingResponse(this);

        public override bool Equals(object other) => Equals(other as PingResponse);

        public bool Equals(PingResponse other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Equals(ClientTimestamp, other.ClientTimestamp) && Equals(ServerTimestamp, other.ServerTimestamp);
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (clientTimestamp_ != null) num ^= ClientTimestamp.GetHashCode();
            if (serverTimestamp_ != null) num ^= ServerTimestamp.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{ClientTimestamp: {ClientTimestamp}, ServerTimestamp: {ServerTimestamp}}}";
        }
    }
}
