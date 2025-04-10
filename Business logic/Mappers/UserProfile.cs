using Models.Entities;
using AutoMapper;
using ViewModels.Model;
using Business_logic.DTOs;

namespace Business_logic.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, AccountLogIn>().ReverseMap();
            CreateMap<User, AccountRegister>().ReverseMap();
            CreateMap<User, ProfileOverviewModel>();
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Picture, opt => opt.Ignore());
        }
    }
}
