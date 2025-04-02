using Models.Entities;
using AutoMapper;
using ViewModels.Model;

namespace Business_logic.Mappers
{
    public class ReviewProfile: Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewViewModel>().
                ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username)).
                ReverseMap().
                ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
