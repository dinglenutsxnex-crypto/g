using Godot;
using System;

namespace clientDTO
{
	public sealed class OfflineRequestData
	{
		private static readonly MessageParser<OfflineRequestData> _parser = new MessageParser<OfflineRequestData>(() => new OfflineRequestData());

		public const int RequestIDFieldNumber = 1;
		private long requestID_;

		public const int CmdFieldNumber = 2;
		private string cmd_ = string.Empty;

		public const int VersionFullFieldNumber = 3;
		private string versionFull_ = string.Empty;

		public const int BinaryFieldNumber = 4;
		private ByteString binary_ = ByteString.Empty;

		public static MessageParser<OfflineRequestData> Parser
		{
			get
			{
				return _parser;
			}
		}

		public long RequestID
		{
			get
			{
				return requestID_;
			}
			set
			{
				requestID_ = value;
			}
		}

		public string Cmd
		{
			get
			{
				return cmd_;
			}
			set
			{
				cmd_ = value;
			}
		}

		public string VersionFull
		{
			get
			{
				return versionFull_;
			}
			set
			{
				versionFull_ = value;
			}
		}

		public ByteString Binary
		{
			get
			{
				return binary_;
			}
			set
			{
				binary_ = value;
			}
		}

		public OfflineRequestData()
		{
		}

		public OfflineRequestData(OfflineRequestData other)
			: this()
		{
			requestID_ = other.requestID_;
			cmd_ = other.cmd_;
			versionFull_ = other.versionFull_;
			binary_ = other.binary_;
		}

		public OfflineRequestData Clone()
		{
			return new OfflineRequestData(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as OfflineRequestData);
		}

		public bool Equals(OfflineRequestData other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(other, this))
			{
				return true;
			}
			if (RequestID != other.RequestID)
			{
				return false;
			}
			if (Cmd != other.Cmd)
			{
				return false;
			}
			if (VersionFull != other.VersionFull)
			{
				return false;
			}
			if (Binary != other.Binary)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (RequestID != 0)
			{
				num ^= RequestID.GetHashCode();
			}
			if (Cmd.Length != 0)
			{
				num ^= Cmd.GetHashCode();
			}
			if (VersionFull.Length != 0)
			{
				num ^= VersionFull.GetHashCode();
			}
			if (Binary.Length != 0)
			{
				num ^= Binary.GetHashCode();
			}
			return num;
		}

		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}
	}
}
