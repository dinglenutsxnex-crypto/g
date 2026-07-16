using System;

namespace common
{
    public sealed class KickEvent : IEquatable<KickEvent>
    {
        private KickReason reason_;
        private string initiator_ = string.Empty;
        private long banMillisLeft_;

        public KickReason Reason { get => reason_; set => reason_ = value; }
        public string Initiator { get => initiator_; set => initiator_ = value ?? string.Empty; }
        public long BanMillisLeft { get => banMillisLeft_; set => banMillisLeft_ = value; }

        public KickEvent() { }

        public KickEvent(KickEvent other)
        {
            reason_ = other.reason_;
            initiator_ = other.initiator_;
            banMillisLeft_ = other.banMillisLeft_;
        }

        public KickEvent Clone() => new KickEvent(this);

        public override bool Equals(object other) => Equals(other as KickEvent);

        public bool Equals(KickEvent other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Reason == other.Reason && Initiator == other.Initiator && BanMillisLeft == other.BanMillisLeft;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (Reason != 0) num ^= Reason.GetHashCode();
            if (Initiator.Length != 0) num ^= Initiator.GetHashCode();
            if (BanMillisLeft != 0) num ^= BanMillisLeft.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{Reason: {Reason}, Initiator: {Initiator}, BanMillisLeft: {BanMillisLeft}}}";
        }
    }
}
