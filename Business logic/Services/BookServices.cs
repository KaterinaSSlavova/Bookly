using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
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

        public bool AddBook(AddBookModel bookModel)
        {
            Book book = _mapper.Map<Book>(bookModel);
            return _ibookRepo.AddBook(book); 
        }

        public List<Book> LoadBooks()
        {
            List<Book> books = _ibookRepo.LoadBooks();
            return books;   
        }

        public List<BookViewModel> GetAllBooksViewModel()
        {
            List<Book> books = LoadBooks();
            List<BookViewModel> model =_mapper.Map<List<BookViewModel>>(books);
            return model;
        }

        public Book? GetBookById(int id)
        {  
            return _ibookRepo.GetBookById(id);
        }

        public void RemoveBook(int id)
        {
            _ibookRepo.RemoveBook(id);
        }

    }
}
