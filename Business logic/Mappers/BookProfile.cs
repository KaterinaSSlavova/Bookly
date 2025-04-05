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

            CreateMap<DateWithABookDTO, DateWithABookViewModel>()
                .ForMember(dest => dest.FilteredBooks, opt => opt.MapFrom(src => src.FilteredBooks))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => MapGenres()))
                .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => MapRatings()))
                .ReverseMap()
                .ForMember(dest => dest.FilteredBooks, opt => opt.MapFrom(src => src.FilteredBooks))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => GetGenres()))
                .ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => GetRatings()));

            CreateMap<AddBookModel, Book>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => Enum.Parse(typeof(Genre), src.Genre)))
                .ForSourceMember(src => src.Genres, opt => opt.DoNotValidate());
        }

        private List<SelectListItem> MapGenres()
        {
            return GetGenres().Select(g => new SelectListItem
            {
                Value = g.ToString(),
                Text = g.ToString()
            }).ToList();
        }

        private List<SelectListItem> MapRatings()
        {
            return GetRatings().Select(r => new SelectListItem
            {
                Value = r.ToString(),
                Text = r.ToString()
            }).ToList();
        }

        private List<Genre> GetGenres()
        {
            return Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
        }

        private List<Ratings> GetRatings()
        {
            return Enum.GetValues(typeof(Ratings)).Cast<Ratings>().ToList();
        }
    }
}
