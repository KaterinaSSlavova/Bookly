using AutoMapper;
using Business_logic.DTOs;
using EFDataLayer.DBContext;
using Bookly.ViewModels;

namespace Bookly.Mappers
{
    public class BookModelMapper: Profile
    {
        public BookModelMapper()
        {
            CreateMap<BookDTO, BookViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => Enum.Parse(typeof(Genre), src.Genre)));


            CreateMap<BookDetailsDTO, BookDetailsViewModel>()
                .ForMember(dest => dest.RatingFromUser, opt => opt.MapFrom(src => src.RatingFromUser.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.RatingFromUser, opt => opt.MapFrom(src => src.RatingFromUser));

            CreateMap<AddBookModel, BookDTO>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => Enum.Parse(typeof(Genre), src.Genre)))
                .ForSourceMember(src => src.Genres, opt => opt.DoNotValidate());

            CreateMap<CurrentBookDTO, CurrentBookViewModel>()
                .ForSourceMember(src => src.User, opt => opt.DoNotValidate())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse(typeof(Status), src.Status)));
        }
    }
}
