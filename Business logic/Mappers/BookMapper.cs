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
            CreateMap<CurrentBook, CurrentBookDTO>()
                .ReverseMap();
            CreateMap<BookDTO, CurrentBookDTO>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

     //       CreateMap<CurrentBookDTO, CurrentBook>()
     //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Book.Id));
        }
    }
}
