using System;

namespace SF3.UserData
{
    public interface IUserDataManager
    {
        void Initialize();
        void UpdateUserData(object dataObject);
    }
}
