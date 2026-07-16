using Godot;
using sf3DTO;

namespace SF3.UserData
{
    public partial class UserShopManager : Node, IUserDataManager
    {
        private static UserShopManager _instance;
        public static UserShopManager Instance => _instance;

        public UserShopConfiguration shopConfiguration { get; }

        public override void _Ready()
        {
            _instance = this;
            UserDataController.AddUserDataManager(UserDataManagerType.Shop, this);
        }

        public void Initialize() { }
        public void UpdateUserData(object dataObject) { }
    }
}
