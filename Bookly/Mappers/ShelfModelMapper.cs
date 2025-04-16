using AutoMapper;
using Business_logic.DTOs;
using ViewModels.Model;

namespace Bookly.Mappers
{
    public class ShelfModelMapper: Profile
    {
        public ShelfModelMapper() 
        {
            CreateMap<ShelfDTO, ShelfViewModel>().ReverseMap();
        }
    }
}
