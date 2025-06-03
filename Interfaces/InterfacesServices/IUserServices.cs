using Business_logic.DTOs;
using Microsoft.AspNetCore.Http;
using EFDataLayer.Entities;

namespace Interfaces
{
    public interface IUserServices
    {
        void Register(UserDTO user);
        bool LogIn(UserDTO user);
        UserDTO? LoadUser();
        UserDTO? GetUserByUsername(string username);
        void UpdateProfile(UserDTO User);
        User ConvertToEntity(UserDTO user);
        UserDTO? ConvertToDTO(User user);
        string ConvertToString(IFormFile image);
    }
}
