using System;

namespace sf3DTO
{
    public sealed class BuyBoosterRequest : IEquatable<BuyBoosterRequest>
    {
        private int shopBoosterModelId_;
        private Currency currency_;

        public int ShopBoosterModelId { get => shopBoosterModelId_; set => shopBoosterModelId_ = value; }
        public Currency Currency { get => currency_; set => currency_ = value; }

        public BuyBoosterRequest() { }

        public BuyBoosterRequest(BuyBoosterRequest other)
        {
            shopBoosterModelId_ = other.shopBoosterModelId_;
            Currency = other.currency_?.Clone();
        }

        public BuyBoosterRequest Clone() => new BuyBoosterRequest(this);

        public override bool Equals(object other) => Equals(other as BuyBoosterRequest);

        public bool Equals(BuyBoosterRequest other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return ShopBoosterModelId == other.ShopBoosterModelId && Equals(Currency, other.Currency);
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (ShopBoosterModelId != 0) num ^= ShopBoosterModelId.GetHashCode();
            if (currency_ != null) num ^= Currency.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{ShopBoosterModelId: {ShopBoosterModelId}, Currency: {Currency}}}";
        }
    }
}
