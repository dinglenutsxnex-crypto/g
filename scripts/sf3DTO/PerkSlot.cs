using System;

namespace sf3DTO
{
    public sealed class PerkSlot : IEquatable<PerkSlot>
    {
        private int slotIndex_;
        private int perkModelId_;

        public int SlotIndex { get => slotIndex_; set => slotIndex_ = value; }
        public int PerkModelId { get => perkModelId_; set => perkModelId_ = value; }

        public PerkSlot() { }

        public PerkSlot(PerkSlot other)
        {
            slotIndex_ = other.slotIndex_;
            perkModelId_ = other.perkModelId_;
        }

        public PerkSlot Clone() => new PerkSlot(this);

        public override bool Equals(object other) => Equals(other as PerkSlot);

        public bool Equals(PerkSlot other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return SlotIndex == other.SlotIndex && PerkModelId == other.PerkModelId;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (SlotIndex != 0) num ^= SlotIndex.GetHashCode();
            if (PerkModelId != 0) num ^= PerkModelId.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{SlotIndex: {SlotIndex}, PerkModelId: {PerkModelId}}}";
        }
    }
}
