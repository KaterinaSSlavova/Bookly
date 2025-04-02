using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using ViewModels.Model;
using AutoMapper;

namespace Bookly.Business_logic.Services
{
    public class BookServices: IBookServices
    {
        private readonly IBookRepository _ibookRepo;
        private readonly IMapper _mapper;
        public BookServices(IBookRepository ibookRepo, IMapper mapper)
        {
            this._ibookRepo = ibookRepo;
            this._mapper = mapper;
        }

        public bool AddBook(BookViewModel bookModel)
        {
            Book book = _mapper.Map<Book>(bookModel);
            return _ibookRepo.AddBook(book); 
        }

        public List<BookViewModel> LoadBooks()
        {
            List<BookViewModel> books = new List<BookViewModel>();
            foreach(Book book in _ibookRepo.LoadBooks())
            {
                books.Add(_mapper.Map<BookViewModel>(book));
            }
            return books;    
        }

        public BookViewModel? GetBookById(int id)
        {
            Book book = _ibookRepo.GetBookById(id);
            BookViewModel bookModel = _mapper.Map<BookViewModel>(book);    
            return bookModel;
        }

        public void RemoveBook(int id)
        {
            _ibookRepo.RemoveBook(id);
        }

        public List<SelectListItem> GetAllGenres()
        {
            List<SelectListItem> genres = Enum.GetValues(typeof(Genre))
                .Cast<Genre>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(), 
                    Text = g.ToString()  
                })
                .ToList();
            return genres;
        }

    }
}
