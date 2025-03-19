using Bookly.Data.Models;
using Bookly.Data.ViewModels;

namespace Bookly.Data.InterfacesRepo
{
    public interface IUserRepository
    {
        bool Register(AccountRegister user);
        User? LogIn(string username, string password);
        User? LoadUser(string username);
        bool UpdateProfile(User user, string picture, string newUsername, int age, string email, string password);
    }
}
