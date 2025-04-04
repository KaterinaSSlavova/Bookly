using Models.Entities;
using AutoMapper;
using ViewModels.Model;

namespace Business_logic.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, AccountLogIn>().ReverseMap();
            CreateMap<User, AccountRegister>().ReverseMap();
            CreateMap<User, ProfileOverviewModel>();
            CreateMap<EditProfileModel, User>()
                .ForMember(dest => dest.Picture, opt => opt.Ignore());
        }
    }
}
