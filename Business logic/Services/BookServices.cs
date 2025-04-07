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
            if (ValidateBook(book)) return false;
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

        public bool ValidateBook(Book newBook)
        {
            if (newBook == null) return false;
            List<Book> allBooks = LoadBooks();
            foreach(Book book in allBooks)
            {
                if (newBook.Picture == book.Picture && newBook.Title == book.Title && newBook.Author == book.Author) return false;
                if (newBook.ISBN == book.ISBN) return false;
            }
            return true;
        }

        public bool RemoveBook(int id)
        {
            return _ibookRepo.RemoveBook(id);
        }

    }
}
