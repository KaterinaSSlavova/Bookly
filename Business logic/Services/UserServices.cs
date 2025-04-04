using Models.Entities;
using Bookly.Data.InterfacesRepo;
using ViewModels.Model;
using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Http;
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

        public bool Register(AccountRegister model)
        {
            User user = _mapper.Map<User>(model);
            return _iuserRepo.Register(user);
        }

        public User? LogIn(AccountLogIn model)
        {
            User user = _mapper.Map<User>(model);
            return _iuserRepo.LogIn(user);
        }

        public ProfileOverviewModel LoadProfile()
        {
            User user = LoadUser();
            return _mapper.Map<ProfileOverviewModel>(user);
        }

        public User? LoadUser()
        {
            string username = _contextAccessor.HttpContext.Session.GetString("Username");
            return _iuserRepo.LoadUser(username);    
        }

        public bool UpdateProfile(EditProfileModel model)
        {
            _contextAccessor.HttpContext.Session.SetString("Username", model.Username);
            byte[] image = ConvertImageToBinary(model.Picture);
            User updatedUser = _mapper.Map<User>(model);
            updatedUser.Id = LoadUser().Id;
            return _iuserRepo.UpdateProfile(updatedUser, image);
        }

        private byte[] ConvertImageToBinary(IFormFile picture)
        {
            if(picture != null)
            {
                using MemoryStream mStream = new MemoryStream();
                picture.CopyTo(mStream);
                return mStream.ToArray();
            }
            return null;
        }
    }
}
