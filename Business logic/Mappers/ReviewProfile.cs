using Models.Entities;
using AutoMapper;
using ViewModels.Model;
using Business_logic.DTOs;

namespace Business_logic.Mappers
{
    public class ReviewProfile: Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDTO>().ReverseMap();

            CreateMap<ReviewDTO, ReviewViewModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
