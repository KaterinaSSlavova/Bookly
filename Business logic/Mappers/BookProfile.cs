using Models.Entities;
using Models.Enums;
using AutoMapper;
using ViewModels.Model;

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
        }
    }
}
