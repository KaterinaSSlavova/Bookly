using Models.Entities;
using Models.Enums;
using AutoMapper;
using ViewModels.Model;
using Business_logic.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Business_logic.Mappers
{
    public class BookProfile: Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => Enum.Parse(typeof(Genre), src.Genre)));


            CreateMap<BookDetailsDTO, BookDetailsViewModel>()
                .ForMember(dest => dest.RatingFromUser, opt => opt.MapFrom(src => src.RatingFromUser.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.RatingFromUser, opt => opt.MapFrom(src => src.RatingFromUser));

            CreateMap<AddBookModel, Book>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => Enum.Parse(typeof(Genre), src.Genre)))
                .ForSourceMember(src => src.Genres, opt => opt.DoNotValidate());
        }
    }
}
