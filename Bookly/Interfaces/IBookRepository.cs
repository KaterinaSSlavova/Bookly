using Bookly.Models;

namespace Bookly.Interfaces
{
    public interface IBookRepository
    {
        bool AddBook(Book book);
        List<Book> LoadBooks();
        Book? GetBookById(int id);
        void RemoveBook(int id);
    }
}
