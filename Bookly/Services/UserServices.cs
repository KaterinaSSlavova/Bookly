using Bookly.Models;
using Bookly.Repository;
using Bookly.ViewModels;

namespace Bookly.Services
{
    public class UserServices
    {
        private readonly UserRepository _userRepo;
        public UserServices(UserRepository userRepo)
        {
            this._userRepo = userRepo;
        }

        public bool Register(AccountRegister user)
        {
            return _userRepo.Register(user);
        }

        public User? LogIn(string username, string password)
        {
            return _userRepo.LogIn(username, password);
        }

        public User? LoadUser(string username)
        {
            return _userRepo.LoadUser(username);    
        }

        public bool UpdateProfile(User user, string picture, string newUsername, int age, string email, string password)
        {
            return _userRepo.UpdateProfile(user, picture, newUsername, age, email, password);
        }
    }
}
