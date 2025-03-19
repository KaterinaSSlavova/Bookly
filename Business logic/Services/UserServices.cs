using Bookly.Data.Models;
using Bookly.Data.InterfacesRepo;
using Bookly.Data.ViewModels;
using Bookly.Business_logic.InterfacesServices;

namespace Bookly.Business_logic.Services
{
    public class UserServices: IUserServices
    {
        private readonly IUserRepository _iuserRepo;
        public UserServices(IUserRepository iuserRepo)
        {
            this._iuserRepo = iuserRepo;
        }

        public bool Register(AccountRegister user)
        {
            return _iuserRepo.Register(user);
        }

        public User? LogIn(string username, string password)
        {
            return _iuserRepo.LogIn(username, password);
        }

        public User? LoadUser(string username)
        {
            return _iuserRepo.LoadUser(username);    
        }

        public bool UpdateProfile(User user, string picture, string newUsername, int age, string email, string password)
        {
            return _iuserRepo.UpdateProfile(user, picture, newUsername, age, email, password);
        }
    }
}
