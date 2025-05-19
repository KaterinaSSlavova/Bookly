using Models.Entities;
using Bookly.Data.InterfacesRepo;
using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Http;
using Business_logic.DTOs;
using System.Text;
using Business_logic.InterfacesHelpers;
using Business_logic.Exceptions;

namespace Bookly.Business_logic.Services
{
    public class UserServices: IUserServices
    {
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPasswordHelper _passwordHelper;
        public UserServices(IUserRepository userRepo, IHttpContextAccessor contextAccessor, IPasswordHelper passwordHelper)
        {
            _userRepo = userRepo;
            _contextAccessor = contextAccessor;
            _passwordHelper = passwordHelper;
        }

        public void Register(UserDTO user)
        {
            ValidateUser(user);
            user.Password = _passwordHelper.HashPassword(user.Password);
            _userRepo.Register(ConvertToEntity(user));
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

        public void UpdateProfile(UserDTO userDTO, IFormFile image)
        {
            UserDTO oldUser = LoadUser();
            ValidateUser(userDTO, oldUser.Id);
            userDTO.Picture = ConvertToString(image);
            userDTO.Id = oldUser.Id;
            _contextAccessor.HttpContext.Session.SetString("Username", userDTO.Username);
            _userRepo.UpdateProfile(ConvertToEntity(userDTO));
        }

        public void UpdateProfile(UserDTO userDTO, string image)
        {
            UserDTO oldUser = LoadUser();
            ValidateUser(userDTO, oldUser.Id);
            userDTO.Picture = image;
            userDTO.Id = oldUser.Id;
            _contextAccessor.HttpContext.Session.SetString("Username", userDTO.Username);
            User user = ConvertToEntity(userDTO);
            _userRepo.UpdateProfile(user);
        }

        public UserDTO? ConvertToDTO(User user)
        {
            if (user == null) return null;
            string picture = user.Picture !=null ? Convert.ToBase64String(user.Picture): null;
            int age = CalculateAge(user);
            return new UserDTO(user.Id, picture, user.Username, user.BirthDate, age, user.Email, user.Password, user.Role);
        }

        public User ConvertToEntity(UserDTO user)
        {
            byte[] picture = user.Picture != null ? Convert.FromBase64String(user.Picture) : null;
            return new User(user.Id, picture, user.Username, user.BirthDate, user.Email, user.Password, user.Role);
        }

        private void ValidateUser(UserDTO userDTO, int? excludedUserId = null)
        {
            if (userDTO == null) 
                throw new ServiceValidationException("Invalid data!");

            if (userDTO.BirthDate.HasValue)
            {
                if (userDTO.BirthDate.Value > DateTime.Now || userDTO.BirthDate.Value.Year == DateTime.Today.Year)
                    throw new InvalidBirthdayException();
            }

            User user = ConvertToEntity(userDTO);

            if (_userRepo.DoesUsernameExists(user, excludedUserId)) 
                throw new UsernameAlreadyExistsException(user.Username);

            if(_userRepo.DoesEmailExists(user, excludedUserId))
                throw new EmailAlreadyExistsException(user.Email);
        }

        private string ConvertToString(IFormFile image)
        {
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }

        }

        public int CalculateAge(User user)
        {
            if(user.BirthDate == null) return 0;

            DateTime today = DateTime.Today;
            DateTime birthDate = user.BirthDate.Value;
            int age = today.Year - birthDate.Year;
            if(birthDate.Month > today.Month || birthDate.Month == today.Month && birthDate.Day > today.Day)
            {
                age--;
            }
            return age;
        }
    }
}
