using System;

namespace sf3DTO
{
    public sealed class EquipRequest : IEquatable<EquipRequest>
    {
        private int modelId_;
        private bool equip_;

        public int ModelId { get => modelId_; set => modelId_ = value; }
        public bool Equip { get => equip_; set => equip_ = value; }

        public EquipRequest() { }

        public EquipRequest(EquipRequest other)
        {
            modelId_ = other.modelId_;
            equip_ = other.equip_;
        }

        public EquipRequest Clone() => new EquipRequest(this);

        public override bool Equals(object other) => Equals(other as EquipRequest);

        public bool Equals(EquipRequest other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return ModelId == other.ModelId && Equip == other.Equip;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (ModelId != 0) num ^= ModelId.GetHashCode();
            if (Equip) num ^= Equip.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{ModelId: {ModelId}, Equip: {Equip}}}";
        }
    }
}
