using System;
using System.Collections.Generic;

namespace sf3DTO
{
    public sealed class RoundRule : IEquatable<RoundRule>
    {
        private int ruleId_;
        private List<RoundRuleAttribute> attrs_ = new List<RoundRuleAttribute>();

        public int RuleId { get => ruleId_; set => ruleId_ = value; }
        public List<RoundRuleAttribute> Attrs => attrs_;

        public RoundRule() { }

        public RoundRule(RoundRule other)
        {
            ruleId_ = other.ruleId_;
            attrs_ = new List<RoundRuleAttribute>(other.attrs_);
        }

        public RoundRule Clone() => new RoundRule(this);

        public override bool Equals(object other) => Equals(other as RoundRule);

        public bool Equals(RoundRule other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return RuleId == other.RuleId;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (RuleId != 0) num ^= RuleId.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{RuleId: {RuleId}, Attrs: {attrs_.Count}}}";
        }
    }
}
