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
                .ForSourceMember(src => src.Image, opt => opt.DoNotValidate())
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDate)))
                .ReverseMap()
                .ForMember(dest => dest.Picture, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Picture))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate.Value.ToString("yyyy-MM-dd")));

            CreateMap<UserDTO, ProfileOverviewModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        }
    }
}
