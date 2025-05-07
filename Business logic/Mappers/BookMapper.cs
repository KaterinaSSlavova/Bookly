using AutoMapper;
using Business_logic.DTOs;
using Models.Entities;

namespace Business_logic.Mappers
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            CreateMap<Book, BookDTO>().ReverseMap();

            CreateMap<CurrentBookDTO, CurrentBook>()
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Book.Id));
        }
    }
}
