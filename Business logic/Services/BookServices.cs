using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using AutoMapper;
using Business_logic.DTOs;
using Business_logic.Exceptions;

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

        public void AddBook(BookDTO bookDTO)
        {
            ValidateBook(bookDTO);
            Book book = _mapper.Map<Book>(bookDTO);
            _bookRepo.AddBook(book); 
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

        public void UpdateBook(BookDTO newBookVersion)
        {
            BookDTO oldBookVersion = GetBookById(newBookVersion.Id);
            if (newBookVersion.Picture == null) newBookVersion.Picture = oldBookVersion.Picture;
            ValidateUpdatedBook(oldBookVersion, newBookVersion);
            _bookRepo.UpdateBook(_mapper.Map<Book>(newBookVersion));
        }

        private void ValidateUpdatedBook(BookDTO oldBookVersion, BookDTO newBookVersion)
        {
            if (newBookVersion == null) throw new ServiceValidationException("Invalid data!");
            if (newBookVersion.Pages == 0) throw new InvalidBookPagesException(newBookVersion.Pages);
            List<BookDTO> allBooks = LoadBooks().Where(b => b.Id != oldBookVersion.Id).ToList();
            foreach(BookDTO book in allBooks)
            {
                if (newBookVersion.ISBN == book.ISBN) throw new DuplicateISBNException(newBookVersion.ISBN);
            }
        }

        private void ValidateBook(BookDTO newBook)
        {
            if (newBook == null) throw new ServiceValidationException("Invalid data!");
            if(newBook.Pages == 0) throw new InvalidBookPagesException(newBook.Pages);
            List<BookDTO> allBooks = LoadBooks();
            foreach(BookDTO book in allBooks)
            {
                if (string.Equals(newBook.ISBN?.Trim(), book.ISBN?.Trim(), StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateISBNException(newBook.ISBN);
            }
        }

        public void RemoveBook(int id)
        {
            if (GetBookById(id) == null) throw new ServiceValidationException("Book was not found.");
            _bookRepo.RemoveBook(id);
        }

    }
}
