using Models.Entities;
using AutoMapper;
using ViewModels.Model;

namespace Business_logic.Mappers
{
    public class ShelfProfile: Profile
    {
        public ShelfProfile()
        {
            CreateMap<Shelf, ShelfViewModel>().ReverseMap();
        }
    }
}
