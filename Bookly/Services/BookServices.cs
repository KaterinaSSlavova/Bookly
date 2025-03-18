using Bookly.Repository;
using Bookly.Models;
using Bookly.Interfaces;

namespace Bookly.Services
{
    public class BookServices: IBookServices
    {
        private readonly IBookRepository _ibookRepo;
        public BookServices(IBookRepository ibookRepo)
        {
            this._ibookRepo = ibookRepo;
        }

        public bool AddBook(Book book)
        {
            return _ibookRepo.AddBook(book); 
        }

        public List<Book> LoadBooks()
        {
            return _ibookRepo.LoadBooks();    
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
