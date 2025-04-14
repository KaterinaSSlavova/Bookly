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
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => Convert.ToBase64String(src.Picture)))
                .ReverseMap()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => Convert.FromBase64String(src.Picture)));

            CreateMap<UserDTO, AccountLogIn>().ReverseMap();
            
            CreateMap<UserDTO, AccountRegister>().ReverseMap();

            CreateMap<AddUserDTO, User>();

            CreateMap<EditProfileModel, UserDTO>()
                .ForSourceMember(src => src.Picture, opt => opt.DoNotValidate());

            CreateMap<UserDTO, ProfileOverviewModel>();
        }
    }
}
