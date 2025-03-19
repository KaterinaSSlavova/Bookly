using Bookly.Data.InterfacesRepo;
using Bookly.Data.Models;
using Bookly.Business_logic.InterfacesServices;

namespace Bookly.Business_logic.Services
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
