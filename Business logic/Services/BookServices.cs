using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using AutoMapper;
using Business_logic.DTOs;

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

        public bool AddBook(BookDTO bookDTO)
        {
            if (!ValidateBook(bookDTO)) return false;
            Book book = _mapper.Map<Book>(bookDTO);
            return _ibookRepo.AddBook(book); 
        }

        public List<BookDTO> LoadBooks()
        {
            List<Book> books = _ibookRepo.LoadBooks();
            return _mapper.Map<List<BookDTO>>(books);
        }

        public BookDTO? GetBookById(int id)
        {  
            Book? book = _ibookRepo.GetBookById(id);
            return _mapper.Map<BookDTO>(book);
        }

        public bool ValidateBook(BookDTO newBook)
        {
            if (newBook == null) return false;
            List<BookDTO> allBooks = LoadBooks();
            foreach(BookDTO book in allBooks)
            {
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
