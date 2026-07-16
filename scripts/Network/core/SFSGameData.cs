using Godot;
using Network.core.data;

namespace Network.core
{
	public class SFSGameData
	{
		protected long _matchID;

		protected long _userID;

		protected bool _profileExists;

		protected AuthManager _auth;

		public long MatchID
		{
			get
			{
				return _matchID;
			}
			set
			{
				_matchID = value;
			}
		}

		public long UserID
		{
			get
			{
				return _userID;
			}
			set
			{
				_userID = value;
			}
		}

		public bool ProfileExists
		{
			get
			{
				return _profileExists;
			}
			set
			{
				_profileExists = value;
			}
		}

		public AuthManager Auth
		{
			get
			{
				return _auth;
			}
		}

		public SFSGameData(AuthManager auth)
		{
			_matchID = -1L;
			_userID = -1L;
			_profileExists = false;
			_auth = auth;
		}

		private bool IsNameSpecified()
		{
			return ProjectSettings.GetSetting("network/user_name", "").AsString() != "";
		}

		public void Clear()
		{
			_auth.Clear();
			if (IsNameSpecified())
			{
				string path = ProjectSettings.GetSetting("network/user_name", "").AsString();
				DirAccess.Remove(path);
			}
		}
	}
}
