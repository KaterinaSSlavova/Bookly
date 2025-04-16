using Models.Entities;
using AutoMapper;
using Business_logic.DTOs;

namespace Business_logic.Mappers
{
    public class ShelfMapper: Profile
    {
        public ShelfMapper()
        {
            CreateMap<ShelfDTO, Shelf>().
                ForSourceMember(src => src.BooksOnShelf, opt => opt.DoNotValidate());
        }
    }
}
