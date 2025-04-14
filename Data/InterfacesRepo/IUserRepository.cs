using Models.Entities;

namespace Bookly.Data.InterfacesRepo
{
    public interface IUserRepository
    {
        bool Register(User user);
        User? LogIn(User user);
        User? LoadUser(string username);
        bool UpdateProfile(User newUse);
        User? GetUserById(int id);
        List<string> GetAllUsernames(User user);
        List<string> GetAllEmails(User user);
    }
}
