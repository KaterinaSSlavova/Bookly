using Models.Entities;
using AutoMapper;
using Business_logic.DTOs;

namespace Business_logic.Mappers
{
    public class BookMapper: Profile
    {
        public BookMapper()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
