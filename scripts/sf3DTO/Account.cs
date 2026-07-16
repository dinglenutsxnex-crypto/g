using System;
using common;

namespace sf3DTO
{
    public sealed class Account : IEquatable<Account>
    {
        private string login_ = string.Empty;
        private AuthType authType_;

        public string Login { get => login_; set => login_ = value ?? string.Empty; }
        public AuthType AuthType { get => authType_; set => authType_ = value; }

        public Account() { }

        public Account(Account other)
        {
            login_ = other.login_;
            authType_ = other.authType_;
        }

        public Account Clone() => new Account(this);

        public override bool Equals(object other) => Equals(other as Account);

        public bool Equals(Account other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Login == other.Login && AuthType == other.AuthType;
        }

        public override int GetHashCode()
        {
            int num = 1;
            if (Login.Length != 0) num ^= Login.GetHashCode();
            if (AuthType != 0) num ^= AuthType.GetHashCode();
            return num;
        }

        public override string ToString()
        {
            return $"{{Login: {Login}, AuthType: {AuthType}}}";
        }
    }
}
