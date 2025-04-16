using AutoMapper;
using Business_logic.DTOs;
using ViewModels.Model;

namespace Bookly.Mappers
{
    public class ReviewModelMapper: Profile
    {
        public ReviewModelMapper()
        {
            CreateMap<ReviewDTO, ReviewViewModel>()
              .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")))
              .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
              .ReverseMap()
              .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
