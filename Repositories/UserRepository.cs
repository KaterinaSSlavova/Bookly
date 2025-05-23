using EFDataLayer.DBContext;
using Interfaces;

namespace Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly BooklyDbContext _context;
        public UserRepository(BooklyDbContext context)
        {
            _context = context;
        }

        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User? LoadUser(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public void UpdateProfile(User newUser)
        {
            _context.Users.Update(newUser);
            _context.SaveChanges();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool DoesUsernameExists(User user)
        {
            return _context.Users.Any(u => u.Username == user.Username);
        }

        public bool DoesEmailExists(User user)
        {
            return _context.Users.Any(u => u.Email == user.Email);
        }
    }
}
