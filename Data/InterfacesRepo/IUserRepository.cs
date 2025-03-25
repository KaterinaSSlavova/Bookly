using Models.Entities;
using ViewModels.Model;

namespace Bookly.Data.InterfacesRepo
{
    public interface IUserRepository
    {
        bool Register(User user);
        User? LogIn(User user);
        User? LoadUser(string username);
        bool UpdateProfile(User user, byte[] picture, string newUsername, int age, string email, string password);
    }
}
