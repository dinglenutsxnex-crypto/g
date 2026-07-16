using System;

namespace sf3DTO
{
    public sealed class OfflineRequest : IEquatable<OfflineRequest>
    {
        private long newStateId_;
        private string cmd_ = string.Empty;
        private string configVersion_ = string.Empty;
        private byte[] data_ = new byte[0];

        public long NewStateId { get => newStateId_; set => newStateId_ = value; }
        public string Cmd { get => cmd_; set => cmd_ = value ?? string.Empty; }
        public string ConfigVersion { get => configVersion_; set => configVersion_ = value ?? string.Empty; }
        public byte[] Data { get => data_; set => data_ = value ?? new byte[0]; }

        public OfflineRequest() { }

        public OfflineRequest(OfflineRequest other)
        {
            newStateId_ = other.newStateId_;
            cmd_ = other.cmd_;
            configVersion_ = other.configVersion_;
            data_ = other.data_;
        }

        public OfflineRequest Clone() => new OfflineRequest(this);

        public override bool Equals(object other) => Equals(other as OfflineRequest);

        public bool Equals(OfflineRequest other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return NewStateId == other.NewStateId && Cmd == other.Cmd && ConfigVersion == other.ConfigVersion;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (NewStateId != 0) num ^= NewStateId.GetHashCode();
            if (Cmd.Length != 0) num ^= Cmd.GetHashCode();
            if (ConfigVersion.Length != 0) num ^= ConfigVersion.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{NewStateId: {NewStateId}, Cmd: {Cmd}, ConfigVersion: {ConfigVersion}}}";
        }
    }
}
