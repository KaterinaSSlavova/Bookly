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
            _ibookRepo = ibookRepo;
            _mapper = mapper;
        }

        public bool AddBook(BookViewModel bookModel)
        {
            Book book = _mapper.Map<Book>(bookModel);
            return _ibookRepo.AddBook(book); 
        }

        public List<BookViewModel> LoadBooks()
        {
            List<Book> books = _ibookRepo.LoadBooks();
            List<BookViewModel> booksModel = _mapper.Map<List<BookViewModel>>(books);
            return booksModel;    
        }

        public Book? GetBookById(int id)
        {  
            return _ibookRepo.GetBookById(id);
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
