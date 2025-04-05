using AutoMapper;
using Business_logic.DTOs;
using Newtonsoft.Json;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using ViewModels.Model;

namespace Bookly.Business_logic.Services
{
    public class DateWithABookServices : IDateWithBookService
    {
        private readonly IMapper _mapper;
        public DateWithABookServices(IMapper mapper)
        {
            this._mapper = mapper;
        }
        public DateWithABookDTO CreateDateDTO(string filteredJson)
        {
            List<Book> filteredBooks = filteredJson != null ? JsonConvert.DeserializeObject<List<Book>>(filteredJson) : new List<Book>();
            return new DateWithABookDTO(filteredBooks);
        }

        public DateWithABookViewModel GetDateWithABookModel(string filteredJson)
        {
            return _mapper.Map<DateWithABookViewModel>(CreateDateDTO(filteredJson));
        }
    }
}

