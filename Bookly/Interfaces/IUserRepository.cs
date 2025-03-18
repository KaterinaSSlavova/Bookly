using Bookly.Models;
using Bookly.ViewModels;

namespace Bookly.Interfaces
{
    public interface IUserRepository
    {
        bool Register(AccountRegister user);
        User? LogIn(string username, string password);
        User? LoadUser(string username);
        bool UpdateProfile(User user, string picture, string newUsername, int age, string email, string password);
    }
}
