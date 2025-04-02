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
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse(typeof(Status), src.Status)));
        }
    }
}
