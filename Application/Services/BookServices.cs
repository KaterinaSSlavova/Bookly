using Interfaces;
using Models.Entities;
using AutoMapper;
using Business_logic.DTOs;
using Exceptions;

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
            ValidateBook(newBookVersion);
            List<BookDTO> allBooks = LoadBooks().Where(b => b.Id != oldBookVersion.Id).ToList();
            foreach(BookDTO book in allBooks)
            {
                if (newBookVersion.ISBN == book.ISBN) throw new DuplicateISBNException(newBookVersion.ISBN);
            }
        }

        private void ValidateNewBookBook(BookDTO newBook)
        {
            ValidateBook(newBook);
            List<BookDTO> allBooks = LoadBooks();
            foreach(BookDTO book in allBooks)
            {
                if (string.Equals(newBook.ISBN?.Trim(), book.ISBN?.Trim(), StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateISBNException(newBook.ISBN);
            }
        }

        private void ValidateBook(BookDTO newBook)
        {
            if (newBook.Title == null) throw new NullReferenceException("Please enter valid book title!");
            if (newBook.Author == null) throw new NullReferenceException("Please enter valid book author!");
            if (newBook.Description == null) throw new NullReferenceException("Please enter valid book description!");
            if (newBook.ISBN == null || newBook.ISBN.StartsWith("-")) throw new NullReferenceException("Please enter valid ISBN!");
            if ((int)newBook.Genre == -1) throw new NullReferenceException("Please select genre!");
            if (newBook.Pages <= 0) throw new InvalidBookPagesException(newBook.Pages);
        }

        public void RemoveBook(int id)
        {
            if (GetBookById(id) == null) throw new NullReferenceException("Book was not found.");
            _bookRepo.RemoveBook(id);
        }

    }
}
