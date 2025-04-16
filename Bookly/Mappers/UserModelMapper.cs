using AutoMapper;
using Business_logic.DTOs;
using Bookly.ViewModels;

namespace Bookly.Mappers
{
    public class UserModelMapper: Profile
    {
        public UserModelMapper()
        {
            CreateMap<UserDTO, AccountLogIn>().ReverseMap();

            CreateMap<UserDTO, AccountRegister>().ReverseMap();

            CreateMap<EditProfileModel, UserDTO>()
                .ForSourceMember(src => src.Picture, opt => opt.DoNotValidate());

            CreateMap<UserDTO, ProfileOverviewModel>();
        }
    }
}
