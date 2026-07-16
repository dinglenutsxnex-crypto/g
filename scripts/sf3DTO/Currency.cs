using System;

namespace sf3DTO
{
    public sealed class Currency : IEquatable<Currency>
    {
        private CurrencyType currencyType_;
        private long value_;

        public CurrencyType CurrencyType { get => currencyType_; set => currencyType_ = value; }
        public long Value { get => value_; set => value_ = value; }

        public Currency() { }

        public Currency(Currency other)
        {
            currencyType_ = other.currencyType_;
            value_ = other.value_;
        }

        public Currency Clone() => new Currency(this);

        public override bool Equals(object other) => Equals(other as Currency);

        public bool Equals(Currency other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return CurrencyType == other.CurrencyType && Value == other.Value;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (CurrencyType != 0) num ^= CurrencyType.GetHashCode();
            if (Value != 0) num ^= Value.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{CurrencyType: {CurrencyType}, Value: {Value}}}";
        }
    }
}
