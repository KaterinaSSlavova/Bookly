using Models.Entities;
using AutoMapper;
using Business_logic.DTOs;
using AutoMapper.Configuration.Conventions;

namespace Business_logic.Mappers
{
    public class ShelfMapper: Profile
    {
        public ShelfMapper()
        {
            CreateMap<RegularShelf, ShelfDTO>()
                .ReverseMap(); 
            CreateMap<CurrentBookShelf, CurrentBookShelfDTO>()
                .ReverseMap();
        }
    }
}
