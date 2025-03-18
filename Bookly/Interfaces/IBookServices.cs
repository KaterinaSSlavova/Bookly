using Bookly.Models;

namespace Bookly.Interfaces
{
    public interface IBookServices
    {
        bool AddBook(Book book);
        List<Book> LoadBooks();
        Book? GetBookById(int id);
        void RemoveBook(int id);
    }
}
