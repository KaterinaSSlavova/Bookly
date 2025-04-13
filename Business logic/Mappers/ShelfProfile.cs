using Models.Entities;
using AutoMapper;
using ViewModels.Model;
using Business_logic.DTOs;

namespace Business_logic.Mappers
{
    public class ShelfProfile: Profile
    {
        public ShelfProfile()
        {
            CreateMap<ShelfDTO, Shelf>().
                ForSourceMember(src => src.BooksOnShelf, opt => opt.DoNotValidate());

            CreateMap<ShelfDTO, ShelfViewModel>().ReverseMap();
        }
    }
}
