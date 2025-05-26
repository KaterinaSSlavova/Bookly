using EFDataLayer.DBContext;
using AutoMapper;
using Business_logic.DTOs;

namespace Business_logic.Mappers
{
    public class ShelfMapper: Profile
    {
        public ShelfMapper()
        {
            CreateMap<Shelf, ShelfDTO>().ReverseMap();

            CreateMap<RegularShelf, RegularShelfDTO>().ReverseMap();

            CreateMap<CurrentBookShelf, CurrentBookShelfDTO>()
                .ReverseMap();
        }
    }
}
