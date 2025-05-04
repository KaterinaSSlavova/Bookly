using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using AutoMapper;
using Business_logic.DTOs;

namespace Bookly.Business_logic.Services
{
    public class BookServices: IBookServices
    {
        private readonly IBookRepository _bookRepo;
        private readonly IMapper _mapper;
        public BookServices(IBookRepository bookRepo, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
        }

        public bool AddBook(BookDTO bookDTO)
        {
            if (!ValidateBook(bookDTO)) return false;
            Book book = _mapper.Map<Book>(bookDTO);
            return _bookRepo.AddBook(book); 
        }

        public List<BookDTO> LoadBooks()
        {
            List<Book> books = _bookRepo.LoadBooks();
            return _mapper.Map<List<BookDTO>>(books);
        }

        public BookDTO? GetBookById(int id)
        {  
            Book? book = _bookRepo.GetBookById(id);
            return _mapper.Map<BookDTO>(book);
        }

        public bool UpdateBook(BookDTO newBookVersion)
        {
            BookDTO oldBookVersion = GetBookById(newBookVersion.Id);
            if (newBookVersion.Picture == null) newBookVersion.Picture = oldBookVersion.Picture;
            if (!ValidateUpdatedBook(oldBookVersion, newBookVersion)) return false;
            return _bookRepo.UpdateBook(_mapper.Map<Book>(newBookVersion));
        }

        private bool ValidateUpdatedBook(BookDTO oldBookVersion, BookDTO newBookVersion)
        {
            List<BookDTO> allBooks = LoadBooks().Where(b => b.Id != oldBookVersion.Id).ToList();
            foreach(BookDTO book in allBooks)
            {
                if (newBookVersion.ISBN == book.ISBN) return false;
            }
            return true;
        }

        private bool ValidateBook(BookDTO newBook)
        {
            if (newBook == null) return false;
            if(newBook.Pages == 0) return false;
            List<BookDTO> allBooks = LoadBooks();
            foreach(BookDTO book in allBooks)
            {
                if (newBook.ISBN == book.ISBN) return false;
            }
            return true;
        }

        public bool RemoveBook(int id)
        {
            return _bookRepo.RemoveBook(id);
        }

    }
}
