using Bookly.Data.Models;
using Bookly.Data.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IUserServices
    {
        bool Register(AccountRegister user);
        User? LogIn(string username, string password);
        User? LoadUser(string username);
        bool UpdateProfile(User user, IFormFile picture, string newUsername, int age, string email, string password);
    }
}
