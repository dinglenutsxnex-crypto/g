using System;
using System.Collections.Generic;

namespace common
{
	public sealed class LoginRequest : IEquatable<LoginRequest>
	{
		public int Version { get; set; }

		public AuthToken PrimaryAuthToken { get; set; }

		public List<AuthToken> SecondaryAuthToken { get; set; }

		public string ExtData { get; set; }

		public LoginRequest()
		{
			SecondaryAuthToken = new List<AuthToken>();
			ExtData = string.Empty;
		}

		public LoginRequest(LoginRequest other)
			: this()
		{
			Version = other.Version;
			PrimaryAuthToken = other.PrimaryAuthToken != null ? other.PrimaryAuthToken.Clone() : null;
			SecondaryAuthToken = new List<AuthToken>(other.SecondaryAuthToken);
			ExtData = other.ExtData;
		}

		public LoginRequest Clone()
		{
			return new LoginRequest(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as LoginRequest);
		}

		public bool Equals(LoginRequest other)
		{
			if (ReferenceEquals(other, null))
			{
				return false;
			}
			if (ReferenceEquals(other, this))
			{
				return true;
			}
			if (Version != other.Version)
			{
				return false;
			}
			if (!Equals(PrimaryAuthToken, other.PrimaryAuthToken))
			{
				return false;
			}
			if (!SecondaryAuthToken.Equals(other.SecondaryAuthToken))
			{
				return false;
			}
			if (ExtData != other.ExtData)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (Version != 0)
			{
				num ^= Version.GetHashCode();
			}
			if (PrimaryAuthToken != null)
			{
				num ^= PrimaryAuthToken.GetHashCode();
			}
			num ^= SecondaryAuthToken.GetHashCode();
			if (ExtData.Length != 0)
			{
				num ^= ExtData.GetHashCode();
			}
			return num;
		}

		public override string ToString()
		{
			return $"LoginRequest[Version={Version}, PrimaryAuthToken={PrimaryAuthToken}, SecondaryAuthToken=[{string.Join(",", SecondaryAuthToken)}], ExtData={ExtData}]";
		}
	}
}
