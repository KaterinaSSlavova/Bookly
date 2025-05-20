using Models.Entities;

namespace Bookly.Data.InterfacesRepo
{
    public interface IUserRepository
    {
        void Register(User user);
        User? LoadUser(string username);
        void UpdateProfile(User newUse);
        User? GetUserById(int id);
        bool DoesUsernameExists(User user);
        bool DoesEmailExists(User user);
    }
}
