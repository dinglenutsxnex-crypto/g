using System;

namespace sf3DTO
{
    public sealed class InsertPerkRequest : IEquatable<InsertPerkRequest>
    {
        private int itemModelId_;
        private int slotIndex_;
        private int perkModelId_;
        private bool insert_;

        public int ItemModelId { get => itemModelId_; set => itemModelId_ = value; }
        public int SlotIndex { get => slotIndex_; set => slotIndex_ = value; }
        public int PerkModelId { get => perkModelId_; set => perkModelId_ = value; }
        public bool Insert { get => insert_; set => insert_ = value; }

        public InsertPerkRequest() { }

        public InsertPerkRequest(InsertPerkRequest other)
        {
            itemModelId_ = other.itemModelId_;
            slotIndex_ = other.slotIndex_;
            perkModelId_ = other.perkModelId_;
            insert_ = other.insert_;
        }

        public InsertPerkRequest Clone() => new InsertPerkRequest(this);

        public override bool Equals(object other) => Equals(other as InsertPerkRequest);

        public bool Equals(InsertPerkRequest other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return ItemModelId == other.ItemModelId && SlotIndex == other.SlotIndex && PerkModelId == other.PerkModelId && Insert == other.Insert;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (ItemModelId != 0) num ^= ItemModelId.GetHashCode();
            if (SlotIndex != 0) num ^= SlotIndex.GetHashCode();
            if (PerkModelId != 0) num ^= PerkModelId.GetHashCode();
            if (Insert) num ^= Insert.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{ItemModelId: {ItemModelId}, SlotIndex: {SlotIndex}, PerkModelId: {PerkModelId}, Insert: {Insert}}}";
        }
    }
}
