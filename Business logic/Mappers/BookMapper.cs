using AutoMapper;
using Business_logic.DTOs;
using EFDataLayer.DBContext;

namespace Business_logic.Mappers
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<CurrentBook, CurrentBookDTO>()
                .ForMember(dest => dest.CurrentProgress, opt => opt.MapFrom(src => src.Progress))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => src.CurrentProgress));
        }
    }
}
