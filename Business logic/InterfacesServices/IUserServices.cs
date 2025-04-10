using Models.Entities;
using Business_logic.DTOs;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IUserServices
    {
        bool Register(User user);
        User? LogIn(User user);
        User? LoadUser();
        bool UpdateProfile(UserDTO User);
    }
}
