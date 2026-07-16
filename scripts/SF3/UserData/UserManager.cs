using Godot;
using sf3DTO;

namespace SF3.UserData
{
    public partial class UserManager : Node, IUserDataManager
    {
        private static UserManager _instance;
        public static UserManager Instance => _instance;
        public static UserModelInfo UserModelInfo => Instance._userModelInfo;
        private UserModelInfo _userModelInfo;

        public override void _Ready()
        {
            _instance = this;
            UserDataController.AddUserDataManager(UserDataManagerType.User, this);
        }

        public void Initialize() { }
        public void UpdateUserData(object playerData) { }
    }
}
