using Models.Entities;
using Bookly.Data.InterfacesRepo;
using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Http;
using Business_logic.DTOs;
using AutoMapper;

namespace Bookly.Business_logic.Services
{
    public class UserServices: IUserServices
    {
        private readonly IUserRepository _iuserRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserServices(IUserRepository iuserRepo, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            this._iuserRepo = iuserRepo;
            this._mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public bool Register(UserDTO user)
        {
            return _iuserRepo.Register(ConvertToEntity(user));
        }

        public bool LogIn(UserDTO user)
        {
            if(_iuserRepo.LogIn(ConvertToEntity(user)) != null)
            {
                return true;
            }
            return false;
        }

        public UserDTO? LoadUser()
        {
            string username = _contextAccessor.HttpContext.Session.GetString("Username");
            User user = _iuserRepo.LoadUser(username);
            return ConvertToDTO(user);  
        }

        public UserDTO? GetUserByUsername(string username)
        {
            User user = _iuserRepo.LoadUser(username);
            return ConvertToDTO(user);
        }

        public bool UpdateProfile(UserDTO userDTO, IFormFile image)
        { 
            if (!ValidateUser(userDTO))
            {
                return false;
            }
            userDTO.Id = LoadUser().Id;
            userDTO.Picture = ConvertToString(image);
            _contextAccessor.HttpContext.Session.SetString("Username", userDTO.Username);
            return _iuserRepo.UpdateProfile(ConvertToEntity(userDTO));
        }

        private UserDTO ConvertToDTO(User user)
        {
            string picture = user.Picture !=null ? Convert.ToBase64String(user.Picture): null;
            return new UserDTO(user.Id, picture, user.Username, user.Age, user.Email, user.Password);
        }

        public User ConvertToEntity(UserDTO user)
        {
            byte[] picture = user.Picture != null ? Convert.FromBase64String(user.Picture) : null;
            return new User(user.Id, picture, user.Username, user.Age, user.Email, user.Password);
        }

        private bool ValidateUser(UserDTO userDTO)
        {
            if (userDTO == null) return false;
            User user = ConvertToEntity(userDTO);
            List<string> usernames = _iuserRepo.GetAllUsernames(user);
            List<string> emails = _iuserRepo.GetAllEmails(user);
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
    }
}
