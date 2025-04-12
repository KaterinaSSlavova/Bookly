using Models.Entities;
using Bookly.Data.InterfacesRepo;
using ViewModels.Model;
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

        public bool Register(User user)
        {
            return _iuserRepo.Register(user);
        }

        public User? LogIn(User user)
        {
            return _iuserRepo.LogIn(user);
        }

        public User? LoadUser()
        {
            string username = _contextAccessor.HttpContext.Session.GetString("Username");
            return _iuserRepo.LoadUser(username);    
        }

        public bool UpdateProfile(UserDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);
            byte[] image = userDTO.Picture;
            _contextAccessor.HttpContext.Session.SetString("Username", user.Username);
            user.SetId(LoadUser().Id);
            if(!ValidateUser(user))
            {
                return false;
            }
            return _iuserRepo.UpdateProfile(user, image);
        }

        private bool ValidateUser(User user)
        {
            if (user == null) return false;
            List<string> usernames = _iuserRepo.GetAllUsernames(LoadUser());
            List<string> emails = _iuserRepo.GetAllEmails(LoadUser());
            if (usernames.Contains(user.Username) || emails.Contains(user.Email)) return false;

            return true;
        }
    }
}
