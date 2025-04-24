using Business_logic.DTOs;
using Microsoft.AspNetCore.Http;
using Models.Entities;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IUserServices
    {
        bool Register(UserDTO user);
        bool LogIn(UserDTO user);
        UserDTO? LoadUser();
        UserDTO? GetUserByUsername(string username);
        bool UpdateProfile(UserDTO User, IFormFile image);
        User ConvertToEntity(UserDTO user);
    }
}
