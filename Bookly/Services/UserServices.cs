using Bookly.Models;
using Bookly.Repository;
using Bookly.ViewModels;
using Bookly.Interfaces;

namespace Bookly.Services
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
