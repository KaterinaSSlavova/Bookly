using Models.Entities;

namespace Bookly.Data.InterfacesRepo
{
    public interface IUserRepository
    {
        void Register(User user);
        User? LoadUser(string username);
        void UpdateProfile(User newUse);
        User? GetUserById(int id);
        List<string> GetAllUsernames(User user);
        List<string> GetAllEmails(User user);
    }
}
