using Interfaces;
using Exceptions;
using EFDataLayer.Entities;

namespace Business_logic.Helpers
{
    public class UserValidation : IUserValidation
    {
        private readonly IUserRepository _userRepo;
        public UserValidation(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public void ValidateUser(User user)
        {
            if (user == null)
                throw new NullReferenceException("Invalid data!");

            if (user.BirthDate.HasValue)
            {
                if (user.BirthDate.Value > DateTime.Now || user.BirthDate.Value.Year == DateTime.Today.Year)
                    throw new InvalidBirthdayException();
            }

            if (_userRepo.DoesUsernameExists(user))
                throw new UsernameAlreadyExistsException(user.Username);

            if (_userRepo.DoesEmailExists(user))
                throw new EmailAlreadyExistsException(user.Email);
        }
    }
}
