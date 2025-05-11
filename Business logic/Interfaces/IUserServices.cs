using Business_logic.DTOs;
using Microsoft.AspNetCore.Http;
using Models.Entities;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IUserServices
    {
        void Register(UserDTO user);
        bool LogIn(UserDTO user);
        UserDTO? LoadUser();
        UserDTO? GetUserByUsername(string username);
        void UpdateProfile(UserDTO User, IFormFile image);
        void UpdateProfile(UserDTO User, string image);
        UserDTO ConvertToDTO(User user);
        User ConvertToEntity(UserDTO user);
    }
}
