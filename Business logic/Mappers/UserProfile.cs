using AutoMapper;
using ViewModels.Model;
using Business_logic.DTOs;

namespace Business_logic.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, AccountLogIn>().ReverseMap();
            
            CreateMap<UserDTO, AccountRegister>().ReverseMap();

            CreateMap<EditProfileModel, UserDTO>()
                .ForSourceMember(src => src.Picture, opt => opt.DoNotValidate());

            CreateMap<UserDTO, ProfileOverviewModel>();
        }
    }
}
