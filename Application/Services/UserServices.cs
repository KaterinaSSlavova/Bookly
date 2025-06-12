using Models.Entities;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Business_logic.DTOs;
using Exceptions;

namespace Bookly.Business_logic.Services
{
    public class UserServices: IUserServices
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserServices(IUserRepository userRepo, IPasswordHelper passwordHelper, IHttpContextAccessor contextAccessor)
        {
            _userRepo = userRepo;
            _passwordHelper = passwordHelper;
            _contextAccessor = contextAccessor;
        }

        public void Register(UserDTO userDTO)
        {
            userDTO.Password = _passwordHelper.HashPassword(userDTO.Password);
            User user = ConvertToEntity(userDTO);
            ValidateUser(user);
            _userRepo.Register(user);
        }

        public bool LogIn(UserDTO loggingUser)
        {
            UserDTO? storedUser = GetUserByUsername(loggingUser.Username);
            if (storedUser == null) return false;
            return _passwordHelper.VerifyPassword(loggingUser.Password, storedUser.Password);
        }

        public UserDTO? LoadUser()
        {
            string username = _contextAccessor.HttpContext.Session.GetString("Username");
            User user = _userRepo.LoadUser(username);
            return ConvertToDTO(user);  
        }

        public UserDTO? GetUserByUsername(string username)
        {
            User user = _userRepo.LoadUser(username);
            return ConvertToDTO(user);
        }

        public void UpdateProfile(UserDTO userDTO)
        {
            User user = ConvertToEntity(userDTO);
            ValidateUser(user);
            _userRepo.UpdateProfile(user);
        }

        public string ConvertToString(IFormFile image)
        {
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        public UserDTO? ConvertToDTO(User user)
        {
            if (user == null) return null;
            string picture = user.Picture != null ? Convert.ToBase64String(user.Picture) : null;
            int age = CalculateAge(user);
            return new UserDTO(user.Id, picture, user.Username, user.BirthDate, age, user.Email, user.Password, user.Role);
        }

        public User ConvertToEntity(UserDTO user)
        {
            byte[] picture = user.Picture != null ? Convert.FromBase64String(user.Picture) : null;
            return new User(user.Id, picture, user.Username, user.BirthDate, user.Email, user.Password, user.Role);
        }

        public int CalculateAge(User user)
        {
            if (user.BirthDate == null) return 0;

            DateTime today = DateTime.Today;
            DateTime birthDate = user.BirthDate.Value;
            int age = today.Year - birthDate.Year;
            if (birthDate.Month > today.Month || birthDate.Month == today.Month && birthDate.Day > today.Day)
            {
                age--;
            }
            return age;
        }

        public void ValidateUser(User user)
        {
            if (user == null)
                throw new NullReferenceException("Invalid data!");

            if(user.Id != 0)
            {
                if (user.BirthDate.HasValue)
                {
                    if (user.BirthDate.Value > DateTime.Now || user.BirthDate.Value.Year == DateTime.Today.Year)
                        throw new InvalidBirthdayException();
                }
                else if (!user.BirthDate.HasValue) throw new InvalidBirthdayException();
            }

            if (user.Username == null) throw new NullReferenceException("Please enter valid data for your username!");

            if (user.Email == null) throw new NullReferenceException("Please enter valid data for your email!");

            if (_userRepo.DoesUsernameExists(user))
                throw new UsernameAlreadyExistsException(user.Username);

            if (_userRepo.DoesEmailExists(user))
                throw new EmailAlreadyExistsException(user.Email);
        }
    }
}
