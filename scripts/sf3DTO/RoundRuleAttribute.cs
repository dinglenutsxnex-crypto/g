using System;

namespace sf3DTO
{
    public sealed class RoundRuleAttribute : IEquatable<RoundRuleAttribute>
    {
        private int attrId_;
        private string attrValue_ = string.Empty;

        public int AttrId { get => attrId_; set => attrId_ = value; }
        public string AttrValue { get => attrValue_; set => attrValue_ = value ?? string.Empty; }

        public RoundRuleAttribute() { }

        public RoundRuleAttribute(RoundRuleAttribute other)
        {
            attrId_ = other.attrId_;
            attrValue_ = other.attrValue_;
        }

        public RoundRuleAttribute Clone() => new RoundRuleAttribute(this);

        public override bool Equals(object other) => Equals(other as RoundRuleAttribute);

        public bool Equals(RoundRuleAttribute other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return AttrId == other.AttrId && AttrValue == other.AttrValue;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (AttrId != 0) num ^= AttrId.GetHashCode();
            if (AttrValue.Length != 0) num ^= AttrValue.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{AttrId: {AttrId}, AttrValue: {AttrValue}}}";
        }
    }
}
