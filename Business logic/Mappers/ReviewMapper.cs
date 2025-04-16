using Models.Entities;
using AutoMapper;
using Business_logic.DTOs;

namespace Business_logic.Mappers
{
    public class ReviewMapper: Profile
    {
        public ReviewMapper()
        {
            CreateMap<Review, ReviewDTO>().ReverseMap();
        }
    }
}
