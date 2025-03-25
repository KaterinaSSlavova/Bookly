using Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IUserServices
    {
        bool Register(AccountRegister model);
        User? LogIn(AccountLogIn model);
        User? LoadUser(string username);
        bool UpdateProfile(User user, IFormFile picture, string newUsername, int age, string email, string password);
    }
}
