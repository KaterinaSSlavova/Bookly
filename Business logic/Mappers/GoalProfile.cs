using AutoMapper;
using Business_logic.DTOs;
using Models.Enums;
using Models.Entities;
using ViewModels.Model;

namespace Business_logic.Mappers
{
    public class GoalProfile: Profile
    {
        public GoalProfile()
        {
            CreateMap<Goal, GoalDTO>().ReverseMap();

            CreateMap<GoalDTO, GoalViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End.ToString("yyyy-MM-dd")))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Status) ? Status.Not_started : Enum.Parse(typeof(Status), src.Status)))
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
