using AutoMapper;
using Business_logic.DTOs;
using Bookly.ViewModels;

namespace Bookly.Mappers
{
    public class ShelfModelMapper: Profile
    {
        public ShelfModelMapper() 
        {
            CreateMap<ShelfDTO, ShelfViewModel>()
                .ReverseMap();

            CreateMap<CurrentBookShelfDTO, CurrentBookShelfViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
