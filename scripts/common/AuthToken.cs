using System;

namespace common
{
    public sealed class AuthToken : IEquatable<AuthToken>
    {
        private AuthType type_;
        private string data_ = string.Empty;

        public AuthType Type { get => type_; set => type_ = value; }
        public string Data { get => data_; set => data_ = value ?? string.Empty; }

        public AuthToken() { }

        public AuthToken(AuthToken other)
        {
            type_ = other.type_;
            data_ = other.data_;
        }

        public AuthToken Clone() => new AuthToken(this);

        public override bool Equals(object other) => Equals(other as AuthToken);

        public bool Equals(AuthToken other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Type == other.Type && Data == other.Data;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (Type != 0) num ^= Type.GetHashCode();
            if (Data.Length != 0) num ^= Data.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{Type: {Type}, Data: {Data}}}";
        }
    }
}
