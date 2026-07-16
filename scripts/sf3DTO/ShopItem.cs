using System;

namespace sf3DTO
{
    public sealed class ShopItem : IEquatable<ShopItem>
    {
        private int modelId_;
        private double stackLevel_;
        private int purchaseCount_;

        public int ModelId { get => modelId_; set => modelId_ = value; }
        public double StackLevel { get => stackLevel_; set => stackLevel_ = value; }
        public int PurchaseCount { get => purchaseCount_; set => purchaseCount_ = value; }

        public ShopItem() { }

        public ShopItem(ShopItem other)
        {
            modelId_ = other.modelId_;
            stackLevel_ = other.stackLevel_;
            purchaseCount_ = other.purchaseCount_;
        }

        public ShopItem Clone() => new ShopItem(this);

        public override bool Equals(object other) => Equals(other as ShopItem);

        public bool Equals(ShopItem other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return ModelId == other.ModelId && StackLevel == other.StackLevel && PurchaseCount == other.PurchaseCount;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (ModelId != 0) num ^= ModelId.GetHashCode();
            if (StackLevel != 0.0) num ^= StackLevel.GetHashCode();
            if (PurchaseCount != 0) num ^= PurchaseCount.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{ModelId: {ModelId}, StackLevel: {StackLevel}, PurchaseCount: {PurchaseCount}}}";
        }
    }
}
