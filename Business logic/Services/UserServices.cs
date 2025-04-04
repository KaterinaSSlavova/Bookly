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
        private readonly IMapper _iMapper;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserServices(IUserRepository iuserRepo, IMapper iMapper, IHttpContextAccessor contextAccessor)
        {
            this._iuserRepo = iuserRepo;
            this._iMapper = iMapper;
            _contextAccessor = contextAccessor;
        }

        public bool Register(AccountRegister model)
        {
            User user = _iMapper.Map<User>(model);
            //ShelfViewModel shelf = new ShelfViewModel()
            //{
            //    Name = "Have Read"
            //};
            //_shelfServices.CreateShelf(shelf);
            return _iuserRepo.Register(user);
        }

        public User? LogIn(AccountLogIn model)
        {
            User user = _iMapper.Map<User>(model);
            return _iuserRepo.LogIn(user);
        }

        public User? LoadUser()
        {
            string username = _contextAccessor.HttpContext.Session.GetString("Username");
            return _iuserRepo.LoadUser(username);    
        }

        public bool UpdateProfile(User user, IFormFile picture, string newUsername, int age, string email, string password)
        {
            byte[] image = ConvertImageToBinary(picture);  
            return _iuserRepo.UpdateProfile(user, image, newUsername, age, email, password);
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
