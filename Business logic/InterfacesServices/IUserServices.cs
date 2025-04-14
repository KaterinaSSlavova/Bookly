using Business_logic.DTOs;
using Microsoft.AspNetCore.Http;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IUserServices
    {
        bool Register(UserDTO user);
        bool LogIn(UserDTO user);
        UserDTO? LoadUser();
        bool UpdateProfile(UserDTO User, IFormFile image);
    }
}
