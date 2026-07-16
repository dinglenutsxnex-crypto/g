using System;

namespace sf3DTO
{
    public sealed class BuyItemRequest : IEquatable<BuyItemRequest>
    {
        private int modelId_;
        private Currency currency_;

        public int ModelId { get => modelId_; set => modelId_ = value; }
        public Currency Currency { get => currency_; set => currency_ = value; }

        public BuyItemRequest() { }

        public BuyItemRequest(BuyItemRequest other)
        {
            modelId_ = other.modelId_;
            Currency = other.currency_?.Clone();
        }

        public BuyItemRequest Clone() => new BuyItemRequest(this);

        public override bool Equals(object other) => Equals(other as BuyItemRequest);

        public bool Equals(BuyItemRequest other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return ModelId == other.ModelId && Equals(Currency, other.Currency);
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (ModelId != 0) num ^= ModelId.GetHashCode();
            if (currency_ != null) num ^= Currency.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{ModelId: {ModelId}, Currency: {Currency}}}";
        }
    }
}
