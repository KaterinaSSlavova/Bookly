using Bookly.Repository;
using Bookly.Models;

namespace Bookly.Services
{
    public class BookServices
    {
        private readonly BookRepository _bookRepo;
        public BookServices(BookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        public bool AddBook(Book book)
        {
            return _bookRepo.AddBook(book); 
        }

        public List<Book> LoadBooks()
        {
            return _bookRepo.LoadBooks();    
        }

        public Book? GetBookById(int id)
        {
            return _bookRepo.GetBookById(id);
        }

    }
}
