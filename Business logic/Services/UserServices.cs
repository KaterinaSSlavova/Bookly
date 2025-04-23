using Models.Entities;
using Bookly.Data.InterfacesRepo;
using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Http;
using Business_logic.DTOs;
using AutoMapper;
using System.Text;

namespace Bookly.Business_logic.Services
{
    public class UserServices: IUserServices
    {
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserServices(IUserRepository userRepo, IHttpContextAccessor contextAccessor)
        {
            _userRepo = userRepo;
            _contextAccessor = contextAccessor;
        }

        public bool Register(UserDTO user)
        {
            return _userRepo.Register(ConvertToEntity(user));
        }

        public bool LogIn(UserDTO user)
        {
            if(_userRepo.LogIn(ConvertToEntity(user)) != null)
            {
                return true;
            }
            return false;
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

        public bool UpdateProfile(UserDTO userDTO, IFormFile image)
        {
            userDTO.Picture = ConvertToString(image);
            if (!ValidateUser(userDTO))
            {
                return false;
            }
            userDTO.Id = LoadUser().Id;
            _contextAccessor.HttpContext.Session.SetString("Username", userDTO.Username);
            return _userRepo.UpdateProfile(ConvertToEntity(userDTO));
        }

        private UserDTO ConvertToDTO(User user)
        {
            string picture = user.Picture !=null ? Convert.ToBase64String(user.Picture): null;
            int age = CalculateAge(user);
            return new UserDTO(user.Id, picture, user.Username, user.BirthDate, age, user.Email, user.Password);
        }

        public User ConvertToEntity(UserDTO user)
        {

            byte[] picture = user.Picture != null ? Convert.FromBase64String(user.Picture) : null;
            return new User(user.Id, picture, user.Username, user.BirthDate, user.Email, user.Password);
        }

        private bool ValidateUser(UserDTO userDTO)
        {
            if (userDTO == null) return false;
            if (userDTO.BirthDate.Value > DateTime.Now || userDTO.BirthDate.Value.Year == DateTime.Today.Year) return false;
            User user = ConvertToEntity(userDTO);
            User oldUser = ConvertToEntity(LoadUser());
            List<string> usernames = _userRepo.GetAllUsernames(oldUser);
            List<string> emails = _userRepo.GetAllEmails(oldUser);
            if (usernames.Contains(userDTO.Username) || emails.Contains(userDTO.Email)) return false;

            return true;
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

        private int CalculateAge(User user)
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
