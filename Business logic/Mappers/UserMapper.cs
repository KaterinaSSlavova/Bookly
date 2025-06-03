using AutoMapper;
using Business_logic.DTOs;
using EFDataLayer.Entities;

namespace Business_logic.Mappers
{
    public class UserMapper: Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Picture, opt => opt.Ignore())
                .ForMember(dest => dest.Age, opt => opt.Ignore())
                .ReverseMap()
                .ConstructUsing(src => new User())
                 .ForMember(dest => dest.Picture, opt => opt.Ignore())
                .ForSourceMember(src => src.Age, opt => opt.DoNotValidate());
        }
    }
}
