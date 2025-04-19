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
                .ForSourceMember(src => src.Picture, opt => opt.DoNotValidate())
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDate)));

            CreateMap<UserDTO, ProfileOverviewModel>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.ToString()));
        }
    }
}
