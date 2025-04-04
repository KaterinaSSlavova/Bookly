using AutoMapper;
using Models.Entities;
using Models.Enums;
using ViewModels.Model;

namespace Business_logic.Mappers
{
    public class GoalProfile: Profile
    {
        public GoalProfile()
        {
            CreateMap<Goal, GoalViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Status) ? Status.Not_started : Enum.Parse(typeof(Status), src.Status)))
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
