using AutoMapper;
using Business_logic.DTOs;
using Bookly.ViewModels;

namespace Bookly.Mappers
{
    public class ShelfModelMapper: Profile
    {
        public ShelfModelMapper() 
        {
            CreateMap<ShelfDTO, ShelfViewModel>().ReverseMap();

            CreateMap<RegularShelfDTO, RegularShelfViewModel>().ReverseMap();   

            CreateMap<CurrentBookShelfDTO, CurrentBookShelfViewModel>()
                .ForMember(dest => dest.DeleteModal, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
